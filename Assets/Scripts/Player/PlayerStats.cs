using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth;
    private int strength;
    private int defense;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Add death handling logic here
    }

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public int GetStrength()
    {
        return strength;
    }
    public int GetDefense()
    {
        return defense;
    }
}