using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats Container")]
    [SerializeField]
    private EntityStats entityStats;

    [Header("Current Stats")]
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int strength;
    [SerializeField]
    private int defense;

    private void Awake()
    {
        if (entityStats != null)
        {
            // Initialize current stats from EntityStats
            maxHealth = entityStats.maxHealth;
            currentHealth = entityStats.GetCurrentHealth();
            strength = entityStats.strength;
            defense = entityStats.defense;
        }
    }

    void Start()
    {
        if (entityStats == null)
        {
            Debug.LogError("EntityStats not assigned to PlayerStats!");
        }
    }

    public void TakeDamage(int damage)
    {
        entityStats.TakeDamage(damage);
        if (!entityStats.IsAlive())
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        entityStats.Heal(amount);
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Add death handling logic here
    }

    // Getters that delegate to EntityStats
    public int CurrentHealth => entityStats.GetCurrentHealth();
    public int MaxHealth => entityStats.GetMaxHealth();
    public int GetStrength() => entityStats.GetStrength();
    public int GetDefense() => entityStats.GetDefense();
    public EntityStats GetEntityStats() => entityStats;
}