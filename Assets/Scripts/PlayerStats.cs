using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat defense;

    public Stat attackSpeed;
    public Stat moveSpeed;
    public Stat jumpForce;
    public Stat dashForce;

    public Stat specialAbilityCharge;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Stunned(float duration)
    {
        moveSpeed.AddModifier(-moveSpeed.GetValue());
        jumpForce.AddModifier(-jumpForce.GetValue());
        dashForce.AddModifier(-dashForce.GetValue());
        attackSpeed.AddModifier(-attackSpeed.GetValue());
        // wait for duration
        moveSpeed.RemoveModifier(-moveSpeed.GetValue());
        jumpForce.RemoveModifier(-jumpForce.GetValue());
        dashForce.RemoveModifier(-dashForce.GetValue());
        attackSpeed.RemoveModifier(-attackSpeed.GetValue());
    }

    



    public void TakeDamage(int damage)
    {
        damage -= defense.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

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
