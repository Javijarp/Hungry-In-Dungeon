using System;
using UnityEngine;

class PlayerPickUp : MonoBehaviour
{
    [SerializeField]
    private Transform carryPoint;
    public BoxCollider2D pickArea;
    [SerializeField]
    private GameObject nearbyItem;
    [SerializeField]
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPickUp(GameObject item)
    {
        if (item.TryGetComponent<IPickUpAble>(out IPickUpAble pickUpAble))
        {
            pickUpAble.OnPickUp(this.gameObject);
            GetComponent<PlayerInventory>().SetPickupTime(); // Add this line
            GetComponent<PlayerInventory>().carriedItem = item.GetComponent<TileEntity>();
            anim.SetBool("isCarrying", true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Colliding With: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("TileEntity"))
        {
            Debug.Log("On: " + collision.gameObject.name);
            nearbyItem = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == nearbyItem)
        {
            nearbyItem = null;
        }
    }

    void Update()
    {
        if (nearbyItem != null && Input.GetKey(KeyCode.Space))
        {
            OnPickUp(nearbyItem);
            nearbyItem = null;
        }
    }
}