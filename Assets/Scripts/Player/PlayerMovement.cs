using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float speed;
    private float jumpForce;
    private float horizontal;
    private bool isFacingRight = true;
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBuffer = 0.1f;
    private float jumpBufferCounter;
    private bool isCrouching = false;
    private bool isDiving = false;
    private bool canDive = false;

    [Header("WallSlideJump")]
    private bool isWallSliding;
    private float wallSlideSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.15f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.2f;

    [Header("Dash")] 
    private bool canDash = true;
    private bool isDashing;
    private float dashTime = 0.2f;
    private float dashCoolDown = 3f;
    private float dashForce;

    [Header("Check")]
    private Rigidbody2D rb;
    private LayerMask groundLayer;
    private Transform wallCheck;
    private LayerMask wallLayer;
    private TrailRenderer tr;
    private BoxCollider2D boxCollider2d;
    private PlayerStats playerStats;
    private PlayerInput playerInput;
    //private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        playerStats = GetComponent<PlayerStats>();
        //animator = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = groundLayer;
        wallCheck = transform.Find("WallCheck");
        speed = playerStats.moveSpeed.GetValue();
        jumpForce = playerStats.jumpForce.GetValue();
        dashForce = playerStats.dashForce.GetValue();
    }

    void Update()
    {
        if (playerStats.moveSpeed.GetValue() != speed)
            speed = playerStats.moveSpeed.GetValue();
        if (playerStats.jumpForce.GetValue() != jumpForce)
            jumpForce = playerStats.jumpForce.GetValue();
        if (playerStats.dashForce.GetValue() != dashForce)
            dashForce = playerStats.dashForce.GetValue();

        EndDive();
        
        if (isDashing || isDiving) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        //animator.SetFloat("Speed", Mathf.Abs(horizontal));


        OnJump();

        OnDash();

        OnCrouch();

        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }
        
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    private void OnMove()
    {
        if (isDashing || isWallJumping || isDiving) return;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void OnJump()
    {
        if (isDashing) return;
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump")){
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            jumpBufferCounter = 0f;
        }
        else if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.25f);
            coyoteTimeCounter = 0f;
        }
    }

    private void OnDash()
    {
        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void OnCrouch()
    {
        if (rb.velocity.y < -20f){
            canDive = true;
            Debug.Log("can dive");
        }
        else {
            canDive = false;
        }
        if (Input.GetButtonDown("Crouch"))
        {
            isWallSliding = false;
            if(canDive){
                isDiving = true;
                rb.velocity =
                    new Vector2(rb.velocity.x,
                        (rb.gravityScale * Physics2D.gravity.y * 2f));
            }
            else {
                transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 0.5f, 1f);
                playerStats.moveSpeed.AddModifier(-(speed/2));
                isCrouching = true;
            }
        }
        else if (Input.GetButtonUp("Crouch") && isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x / 1.5f, transform.localScale.y * 2f, 1f);
            playerStats.moveSpeed.RemoveModifier(-(speed));
            isCrouching = false;
        }

    }

    private void EndDive()
    {
        if (IsGrounded())
        {
            isDiving = false;
        }
    }

    private void CapFallSpeed()
    {
        if(isDiving) return;
        if (rb.velocity.y < -60f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -60f);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit =
            Physics2D
                .BoxCast(boxCollider2d.bounds.center,
                boxCollider2d.bounds.size,
                0f,
                Vector2.down,
                0.1f,
                groundLayer);
        return hit.collider != null;
    }

    private bool IsWalled()
    {
        if(isFacingRight)
        {
        RaycastHit2D hit =
            Physics2D
                .BoxCast(boxCollider2d.bounds.center,
                boxCollider2d.bounds.size,
                0f,
                Vector2.right,
                0.5f,
                groundLayer);
        return hit.collider != null;
        }
        else
        {
            RaycastHit2D hit =
                Physics2D
                    .BoxCast(boxCollider2d.bounds.center,
                        boxCollider2d.bounds.size,
                        0f,
                        Vector2.left,
                        0.1f,
                        groundLayer);
            return hit.collider != null;
        }
    }

    private void WallSlide()
    {
        if (isDashing) return;
        if (IsWalled() && !IsGrounded() && rb.velocity.y < 0f && (horizontal > 0f || horizontal < 0f))
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isDashing) return;
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = transform.localScale.x * -1f;
            wallJumpCounter = wallJumpTime;

           CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if (wallJumpCounter > 0f && Input.GetButtonDown("Jump"))
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * speed, jumpForce);
            wallJumpCounter = 0f;
            if(wallJumpDirection != transform.localScale.x)
                {
                    isFacingRight = !isFacingRight;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
        }

        
        Invoke(nameof(StopWallJumping), wallJumpDuration);
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (horizontal > 0 && !isFacingRight || horizontal < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        //animator.SetBool("isDashing", true);
        canDash = false;
        tr.emitting = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (isFacingRight) rb.velocity = new Vector2(dashForce, 0f);
        else rb.velocity = new Vector2(-dashForce, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        //animator.SetBool("isDashing", false);
        canDash = true;
    }

    public bool getIsFacingRight(){return isFacingRight;}

    public bool getIsDashing(){return isDashing;}
}
