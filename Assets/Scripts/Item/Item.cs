using UnityEngine;

class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    private Sprite itemIcon;

    public string ItemName => itemName;
    public Sprite ItemIcon => itemIcon;

    // Range where the items starts to go to the player
    public float hoverRange = 1f;

    // Additional properties and methods for the Item class can be added here
}