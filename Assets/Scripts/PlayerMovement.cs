using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private int speed;
    private int jumpForce;
    private float horizontal;
    private bool isFacingRight = true;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("WallSlideJump")]
    private bool isWallSliding;
    private float wallSlideSpeed = 4f;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.1f;





    [Header("Dash")]
    private bool canDash = true;
    private bool isDashing;
    private float dashTime = 0.2f;
    private float dashCoolDown = 3f;
    private int dashForce;
    private Vector2 dashDirection;

    
    
    private Rigidbody2D rb;

    private LayerMask groundLayer;

    private Transform wallCheck;

    private LayerMask wallLayer;

    private TrailRenderer tr;

    private BoxCollider2D boxCollider2d;

    private PlayerStats playerStats;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        playerStats = GetComponent<PlayerStats>();
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = groundLayer;
        wallCheck = transform.Find("WallCheck");
        speed = playerStats.moveSpeed.GetValue();
        jumpForce = playerStats.jumpForce.GetValue();
        dashForce = playerStats.dashForce.GetValue();
    }

    void Update()
    {
        if (isDashing) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        if (playerStats.moveSpeed.GetValue() != speed)
            speed = playerStats.moveSpeed.GetValue();
        if (playerStats.jumpForce.GetValue() != jumpForce)
            jumpForce = playerStats.jumpForce.GetValue();
        if (playerStats.dashForce.GetValue() != dashForce)
            dashForce = playerStats.dashForce.GetValue();

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }

        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isWallSliding = false;
            transform.localScale = new Vector3(transform.localScale.x * 1.25f, transform.localScale.y - 0.25f, 1f);
            rb.velocity =
                new Vector2(rb.velocity.x,
                    -jumpForce * 0.5f - playerStats.defense.GetValue() * 2f);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            transform.localScale = new Vector3(transform.localScale.x / 1.25f, transform.localScale.y + 0.25f, 1f);
        }

        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }
        
    }

    private void FixedUpdate()
    {
        if (isDashing || isWallJumping) return;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit =
            Physics2D
                .BoxCast(boxCollider2d.bounds.center,
                boxCollider2d.bounds.size,
                0f,
                Vector2.down,
                1f,
                groundLayer);
        return hit.collider != null;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
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

        if (wallJumpCounter > 0f && Input.GetButtonDown("Jump") )
        {
            isWallJumping = true; 
            rb.velocity = new Vector2(wallJumpDirection * speed*1.5f, jumpForce);
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
        canDash = false;
        tr.emitting = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        dashDirection = new Vector2(horizontal, Input.GetAxisRaw("Vertical"));
        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(transform.localScale.x, 0f);
        }
        if (dashDirection.x == 0 && dashDirection.y == 1)
            rb.velocity = dashDirection.normalized * (dashForce * 0.75f);
        else
            rb.velocity = dashDirection.normalized * dashForce;
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}
