using UnityEngine;

public class MeteorObstacle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float lifeTime = 1f;

    [Header("Damage")]
    [SerializeField] private int damage = 34;
    [SerializeField] private GameObject hitExplosionPrefab;

    private float moveDirectionX;
    private bool initialized = false;

    public void Initialize(bool moveRight, float speed)
    {
        moveDirectionX = moveRight ? 1f : -1f;
        moveSpeed = speed;
        initialized = true;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (!initialized) return;

        transform.position += new Vector3(moveDirectionX * moveSpeed * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!initialized) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                bool tookDamage = playerHealth.TakeDamage(damage);

                if (tookDamage)
                {
                    SpawnHitExplosion(transform.position);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void SpawnHitExplosion(Vector3 spawnPosition)
    {
        if (hitExplosionPrefab == null)
            return;

        Instantiate(hitExplosionPrefab, spawnPosition, Quaternion.identity);
    }
}