using UnityEngine;
public class PlayerMovementScript : MonoBehaviour
{

    Vector2 direction;
    [Header("Assignables")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Header("Settings")]
    public float speed = 5f;

    void Awake()
    {
        // Initialize the direction vector
        direction = Vector2.zero;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        direction.y = Input.GetAxisRaw("Vertical");
        direction.x = Input.GetAxisRaw("Horizontal");

        if (direction != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (direction.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = true;
        }

        direction.Normalize();
        playerTransform.Translate(direction * speed * Time.deltaTime);
    }
}
