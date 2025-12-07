using UnityEngine;

interface IPickUpAble
{
    void OnPickUp(GameObject picker);
    void onDrop();
    void onThrow();
    bool PickedUp { get; set; }
}