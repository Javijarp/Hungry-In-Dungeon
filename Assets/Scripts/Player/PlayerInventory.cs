using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Components")]
    [SerializeField]
    private Transform carryPoint;
    public BoxCollider2D pickArea;

    [Header("Stats Reference")]
    [SerializeField]
    private PlayerStats playerStats;

    [Header("Inventory")]
    [SerializeField]
    Item[] items;
    public TileEntity carriedItem;

    private float pickupCooldown = 0.2f;
    private float lastPickupTime = -1f;

    void Start()
    {
        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (carriedItem != null && Input.GetKeyUp(KeyCode.E))
        {
            if (Time.time - lastPickupTime > pickupCooldown)
            {
                DropItem();
            }
        }

        if (carriedItem != null && Input.GetKeyUp(KeyCode.Space))
        {
            if (Time.time - lastPickupTime > pickupCooldown)
            {
                ThrowItem();
            }
        }
    }

    public void PickupItem(TileEntity item)
    {
        carriedItem = item;
        carriedItem.transform.SetParent(carryPoint);
        carriedItem.transform.localPosition = Vector3.zero;
        SetPickupTime();
        GetComponent<Animator>().SetBool("isCarrying", true);

        // Apply item stats when picked up
        ApplyItemStats(item);
    }

    private void ApplyItemStats(TileEntity item)
    {
        IPickUpAble pickupable = item.gameObject.GetComponent<IPickUpAble>();
        if (pickupable is Item itemComponent)
        {
            // Modify player stats based on item properties
            playerStats.GetEntityStats().AddStrength(itemComponent.strengthBonus);
            playerStats.GetEntityStats().AddDefense(itemComponent.defenseBonus);
            playerStats.GetEntityStats().AddMaxHealth(itemComponent.healthBonus);

            Debug.Log($"Picked up {itemComponent.name} - Stats modified!");
        }
    }

    private void DropItem()
    {
        if (carriedItem == null) return;

        // Remove item stats when dropped
        RemoveItemStats(carriedItem);

        carriedItem.gameObject.GetComponent<IPickUpAble>().onDrop();
        carriedItem.transform.position = carryPoint.position + new Vector3(0, -1, 0);
        carriedItem = null;
        GetComponent<Animator>().SetBool("isCarrying", false);
    }

    private void ThrowItem()
    {
        if (carriedItem == null) return;

        RemoveItemStats(carriedItem);

        // Call onThrow BEFORE setting carriedItem to null
        carriedItem.gameObject.GetComponent<IPickUpAble>().onThrow();

        carriedItem = null;
        GetComponent<Animator>().SetBool("isCarrying", false);
    }

    private void RemoveItemStats(TileEntity item)
    {
        if (playerStats == null || playerStats.GetEntityStats() == null)
        {
            Debug.LogWarning("PlayerStats or EntityStats is null, skipping stat removal");
            return;
        }

        IPickUpAble pickupable = item.gameObject.GetComponent<IPickUpAble>();
        if (pickupable is Item itemComponent)
        {
            // Subtract item stats when dropped
            playerStats.GetEntityStats().AddStrength(-itemComponent.strengthBonus);
            playerStats.GetEntityStats().AddDefense(-itemComponent.defenseBonus);
            playerStats.GetEntityStats().AddMaxHealth(-itemComponent.healthBonus);

            Debug.Log($"Dropped {itemComponent.name} - Stats reverted!");
        }
    }

    public void SetPickupTime()
    {
        lastPickupTime = Time.time;
    }
}