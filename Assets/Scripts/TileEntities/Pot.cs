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
            rbCheck.bodyType = RigidbodyType2D.Kinematic; // Disable physics while carried
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
            rbCheck.bodyType = RigidbodyType2D.Kinematic; // Re-enable physics for throw
        }
    }

    public void onThrow()
    {
        if (currentPicker == null)
        {
            Debug.LogError("Cannot throw pot - currentPicker is null!");
            return;
        }

        // Get player stats with null check
        PlayerStats playerStats = currentPicker.GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found on picker!");
            return;
        }

        int playerStrength = playerStats.GetStrength();

        // Calculate throw force based on player strength
        float adjustedThrowForce = throwForce * (playerStrength);

        // Get player's facing direction
        SpriteRenderer spriteRenderer = currentPicker.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on picker!");
            return;
        }

        Vector2 throwDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;

        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rbCheck))
        {
            rbCheck.bodyType = RigidbodyType2D.Dynamic;
            rbCheck.linearVelocity = throwDirection * adjustedThrowForce;
        }

        isThrown = true;

        // Unparent and drop the pot AFTER all references are used
        transform.SetParent(null);
        currentPicker = null;
        PickedUp = false;
    }
}