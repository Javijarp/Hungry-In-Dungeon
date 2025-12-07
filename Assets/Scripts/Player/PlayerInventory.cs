using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Components")]
    [SerializeField]
    private Transform carryPoint;
    public BoxCollider2D pickArea;

    [Header("Inventory")]
    [SerializeField]
    Item[] items;
    public TileEntity carriedItem;

    private float pickupCooldown = 0.2f; // 200ms delay before allowing drop
    private float lastPickupTime = -1f;

    void Update()
    {
        if (carriedItem != null && Input.GetKeyUp(KeyCode.E))
        {
            // Check if enough time has passed since pickup
            if (Time.time - lastPickupTime > pickupCooldown)
            {
                // Implement drop logic here
                carriedItem.gameObject.GetComponent<IPickUpAble>().onDrop();
                carriedItem.transform.position = carryPoint.position + new Vector3(0, -1, 0);
                carriedItem = null;
                GetComponent<Animator>().SetBool("isCarrying", false);
            }
        }

        if (carriedItem != null && Input.GetKeyUp(KeyCode.Space))
        {
            if (carriedItem != null && Input.GetKeyUp(KeyCode.Space))
            {
                if (Time.time - lastPickupTime > pickupCooldown)
                {
                    carriedItem.gameObject.GetComponent<IPickUpAble>().onThrow();
                    carriedItem = null;
                    GetComponent<Animator>().SetBool("isCarrying", false);
                }
            }
        }
    }

    public void SetPickupTime()
    {
        lastPickupTime = Time.time;
    }
}