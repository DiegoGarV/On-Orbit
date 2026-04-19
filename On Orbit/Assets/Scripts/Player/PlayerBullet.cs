using UnityEngine;
using UnityEngine.Pool;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 10;

    private Rigidbody2D rb;
    private IObjectPool<PlayerBullet> pool;
    private bool isActiveBullet;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPool(IObjectPool<PlayerBullet> bulletPool)
    {
        pool = bulletPool;
    }

    public void Launch(Vector2 direction)
    {
        isActiveBullet = true;
        rb.linearVelocity = direction.normalized * speed;

        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActiveBullet) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (!isActiveBullet) return;

        isActiveBullet = false;
        rb.linearVelocity = Vector2.zero;
        CancelInvoke();

        if (pool != null)
            pool.Release(this);
        else
            gameObject.SetActive(false);
    }
}