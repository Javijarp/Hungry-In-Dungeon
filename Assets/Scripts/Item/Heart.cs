using UnityEngine;

class Heart : Item
{
    [SerializeField]
    private int healthAmount;

    public int HealthAmount => healthAmount;


}