using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private LayerMask enemyLayer;
    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private float timeBtwAttack;
    private float vertical;
    private bool isFacingRight;
    private BoxCollider2D boxCollider2d;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Player");
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        timeBtwAttack = playerStats.attackSpeed.GetValue();
        isFacingRight = playerMovement.getIsFacingRight();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        isFacingRight = playerMovement.getIsFacingRight();

        if (timeBtwAttack <= 0)
        {
            if (Input.GetButtonDown("BasicAttack"))
            {
                Attack();
                timeBtwAttack = playerStats.attackSpeed.GetValue();
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
        
    }

    private void Attack()
    {
        if(playerMovement.getIsDashing()) return;
        animator.SetTrigger("Attack");
        RaycastHit2D[] hitEnemies = Physics2D.BoxCastAll(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.zero, 0f, enemyLayer);
        if (vertical > 0f)
        {
            hitEnemies = Physics2D.BoxCastAll(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.up, playerStats.attackRange.GetValue(), enemyLayer);
        }
        else if (vertical < 0f)
        {
            hitEnemies = Physics2D.BoxCastAll(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, playerStats.attackRange.GetValue(), enemyLayer);
        }
        else if (isFacingRight)
        {
            hitEnemies = Physics2D.BoxCastAll(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.right, playerStats.attackRange.GetValue(), enemyLayer);
        }
        else if (!isFacingRight)
        {
            hitEnemies = Physics2D.BoxCastAll(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.left, playerStats.attackRange.GetValue(), enemyLayer);
        }


        foreach (RaycastHit2D enemy in hitEnemies)
        {
            if (enemy.transform == transform) continue;
            enemy.transform.GetComponent<PlayerStats>().TakeDamage(playerStats.damage.GetValue());
        }
    }
}
