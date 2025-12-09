using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Stats", menuName = "Stats/Entity Stats")]
public class EntityStats : ScriptableObject
{
    [Header("Base Stats")]
    public int maxHealth = 100;
    [SerializeField]
    private int currentHealth;
    public int strength = 10;
    public int defense = 5;

    private void OnEnable()
    {
        // Initialize currentHealth when the ScriptableObject is loaded
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log($"Entity took {damage} damage. Current Health: {currentHealth}");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log($"Entity healed {amount}. Current Health: {currentHealth}");
    }

    // Stat modification methods for items
    public void AddStrength(int amount) => strength += amount;
    public void AddDefense(int amount) => defense += amount;
    public void AddMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount; // Also increase current health
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public int GetStrength() => strength;
    public int GetDefense() => defense;
    public bool IsAlive() => currentHealth > 0;
}