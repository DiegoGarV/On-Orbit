using UnityEngine;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour
{
    [Header("Runtime Bullet Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damage = 10;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject explosionPrefab;

    private Rigidbody2D rb;
    private IObjectPool<EnemyBullet> pool;
    private bool isActiveBullet = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void SetPool(IObjectPool<EnemyBullet> bulletPool)
    {
        pool = bulletPool;
    }

    public void Launch(EnemyData enemyData, Vector2 direction)
    {
        if (enemyData != null)
        {
            speed = enemyData.bulletSpeed;
            lifeTime = enemyData.bulletLifeTime;
            damage = enemyData.bulletDamage;

            if (spriteRenderer != null && enemyData.bulletSprite != null)
            {
                spriteRenderer.sprite = enemyData.bulletSprite;
            }
        }

        isActiveBullet = true;
        rb.linearVelocity = direction.normalized * speed;

        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActiveBullet) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                bool tookDamage = playerHealth.TakeDamage(damage);

                if (tookDamage)
                {
                    SpawnHitExplosion(transform.position);
                    ReturnToPool();
                }
            }
        }
    }

    private void SpawnHitExplosion(Vector3 spawnPosition)
    {
        if (explosionPrefab == null)
            return;

        Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
    }


    private void ReturnToPool()
    {
        if (!isActiveBullet) return;

        isActiveBullet = false;
        rb.linearVelocity = Vector2.zero;
        CancelInvoke();

        if (pool != null)
        {
            pool.Release(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}