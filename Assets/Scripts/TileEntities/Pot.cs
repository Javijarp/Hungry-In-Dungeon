using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Pot : TileEntity, IPickUpAble
{
    public GameObject currentPicker;
    public bool PickedUp { get; set; }

    [SerializeField]
    private float throwForce = 10f;
    [SerializeField]
    private float friction = 0.95f; // Friction multiplier per frame

    private Rigidbody2D rb;
    private bool isThrown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Apply friction every frame when thrown
        if (isThrown && rb.linearVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity *= friction; // Multiply velocity by friction each frame
        }
        else if (rb.linearVelocity.magnitude <= 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            isThrown = false;
        }
    }

    public void OnBreak()
    {
        base.onBreak();
    }

    public void OnPickUp(GameObject picker)
    {
        currentPicker = picker;
        Debug.Log("Pot picked up by: " + currentPicker.name);
        transform.SetParent(currentPicker.transform.Find("CarryPoint"));
        transform.localPosition = Vector3.zero;
        PickedUp = true;
        isThrown = false;

        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rbCheck))
        {
            rbCheck.linearVelocity = Vector2.zero;
            rbCheck.isKinematic = true; // Disable physics while held
        }
    }

    public void onDrop()
    {
        Debug.Log("Pot dropped by: " + currentPicker.name);
        currentPicker = null;
        transform.SetParent(null);
        PickedUp = false;
        isThrown = false;

        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rbCheck))
        {
            rbCheck.isKinematic = false; // Re-enable physics
        }
    }

    public void onThrow()
    {
        // Get player stats
        PlayerStats playerStats = currentPicker.GetComponent<PlayerStats>();
        float playerStrength = playerStats != null ? playerStats.GetStrength() : 1f;

        // Calculate throw force based on player strength and pot weight
        float adjustedThrowForce = throwForce * (playerStrength / weight);

        // Get player's facing direction
        Vector2 throwDirection = currentPicker.GetComponent<SpriteRenderer>().flipX ? Vector2.right : Vector2.left;

        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rbCheck))
        {
            rbCheck.isKinematic = false; // Re-enable physics for throw
        }

        // Apply velocity to the pot
        rb.linearVelocity = throwDirection * adjustedThrowForce * 20f;
        isThrown = true;

        // Unparent and drop the pot
        transform.SetParent(null);
        currentPicker = null;
        PickedUp = false;
    }
}