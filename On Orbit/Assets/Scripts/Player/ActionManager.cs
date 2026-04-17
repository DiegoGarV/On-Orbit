using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("World Bounds")]
    [SerializeField] private float minX = -2.5f;
    [SerializeField] private float maxX = 2.5f;
    [SerializeField] private float minY = -4.5f;
    [SerializeField] private float maxY = 4.5f;

    [Header("Shooting")]
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireCooldown = 0.15f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float fireTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
        HandleShootingCooldown();

        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        rb.position = clampedPosition;
    }

    private void HandleInput()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.A))
            moveX = -1f;
        else if (Input.GetKey(KeyCode.D))
            moveX = 1f;

        if (Input.GetKey(KeyCode.S))
            moveY = -1f;
        else if (Input.GetKey(KeyCode.W))
            moveY = 1f;

        moveInput = new Vector2(moveX, moveY).normalized;
    }

    private void HandleShootingCooldown()
    {
        if (fireTimer > 0f)
            fireTimer -= Time.deltaTime;
    }

    private void TryShoot()
    {
        if (fireTimer > 0f)
            return;

        fireTimer = fireCooldown;

        if (bulletPrefab == null || leftFirePoint == null || rightFirePoint == null)
        {
            Debug.Log("Falta asignar BulletPrefab o uno de los FirePoints.");
            return;
        }

        Instantiate(bulletPrefab, leftFirePoint.position, leftFirePoint.rotation);
        Instantiate(bulletPrefab, rightFirePoint.position, rightFirePoint.rotation);
    }
}