using UnityEngine;

public class Item : MonoBehaviour, IPickUpAble
{
    [Header("Item Stats")]
    public int strengthBonus = 0;
    public int defenseBonus = 0;
    public int healthBonus = 0;

    public bool PickedUp { get; set; }

    public void onPickUp() { }
    public void onDrop() { }
    public void onThrow() { }

    public void OnPickUp(GameObject picker)
    {

    }
}