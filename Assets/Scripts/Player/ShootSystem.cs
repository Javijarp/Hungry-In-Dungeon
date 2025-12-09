using UnityEditor.Experimental.GraphView;
using UnityEngine;

class ShootingSystem : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private PlayerStats playerStats;
    [Header("Settings")]
    public float projectileSpeed = 10f;
    public int damage = 1;
    [Header("Debug")]
    public bool debugMode = false;
    public bool debugShowShootPoint = false;

    void Awake()
    {
        playerStats = playerStats ?? GetComponent<PlayerStats>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            shootPoint.position = transform.position + Vector3.up;
            Shoot(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            shootPoint.position = transform.position + Vector3.down;
            Shoot(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            shootPoint.position = transform.position + Vector3.left;
            Shoot(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            shootPoint.position = transform.position + Vector3.right;
            Shoot(Vector2.right);
        }
    }

    public void Shoot(Vector2 direction)
    {
        Debug.Log($"Shooting in {direction} direction.");
    }
}