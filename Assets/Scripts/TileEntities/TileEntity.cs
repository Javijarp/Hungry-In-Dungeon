using UnityEngine;

public class TileEntity : MonoBehaviour
{
    [SerializeField]
    protected Loot[] lootTable;

    protected float weight => 1.0f;
    protected void onBreak()
    {
        foreach (Loot loot in lootTable)
        {
            if (Random.Range(0, 100) <= loot.dropChance)
            {
                int amountToDrop = Random.Range(loot.minAmount, loot.maxAmount + 1);
                for (int i = 0; i < amountToDrop; i++)
                {
                    Instantiate(loot.itemPrefab, transform.position, Quaternion.identity);
                }
            }
        }
        Destroy(gameObject);
    }
}