using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 10;
    public float currentHealth { get; private set; }

    [Header("Basic Attack")]
    public Stat damage;
    public Stat attackSpeed;
    public Stat attackRange;

    [Header("Movement")]
    public Stat moveSpeed;
    public Stat jumpForce;
    public Stat dashForce;

    [Header("Special Ability")]
    public Stat specialAbilityCharge;
    public Stat specialAbilityDamage;
    public Stat specialAbilityRange;

    [Header("Ultimate Ability")]
    public Stat ultimateCharge;
    public Stat ultimateDamage;
    public Stat ultimateRange;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Stunned(float duration)
    {
        moveSpeed.AddModifier(-100);
        jumpForce.AddModifier(-100);
        dashForce.AddModifier(-100);
        attackSpeed.AddModifier(-100);
        StartCoroutine(StunTimer(duration));
    }

    private IEnumerator StunTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        moveSpeed.RemoveModifier(-100);
        jumpForce.RemoveModifier(-100);
        dashForce.RemoveModifier(-100);
        attackSpeed.RemoveModifier(-100);
    }
    



    public void TakeDamage(float damage)
    {
        damage = Mathf.Clamp(damage, 0, float.MaxValue);

        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // disable the player and ragdoll it
        Debug.Log(transform.name + " died.");

    }
}
