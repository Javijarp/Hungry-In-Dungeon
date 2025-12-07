using UnityEngine;

[System.Serializable]
public class Loot
{
    public GameObject itemPrefab;
    [Range(0, 100)]
    public int dropChance; // Value between 0 and 1
    public int minAmount;
    public int maxAmount;
}