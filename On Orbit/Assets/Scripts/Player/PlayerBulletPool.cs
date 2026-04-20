using UnityEngine;
using UnityEngine.Pool;

public class PlayerBulletPool : MonoBehaviour
{
    [SerializeField] private PlayerBullet bulletPrefab;
    [SerializeField] private EnemyHitExplotionPool EnemyHitExplotionPool;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;
    [SerializeField] private bool collectionCheck = true;

    private ObjectPool<PlayerBullet> pool;

    public ObjectPool<PlayerBullet> Pool => pool;

    private void Awake()
    {
        pool = new ObjectPool<PlayerBullet>(
            CreateBullet,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            collectionCheck,
            defaultCapacity,
            maxSize
        );
    }

    private PlayerBullet CreateBullet()
    {
        PlayerBullet bullet = Instantiate(bulletPrefab, transform);
        bullet.gameObject.SetActive(false);
        bullet.SetPool(pool);
        bullet.SetEnemyHitExplotionPool(EnemyHitExplotionPool);
        return bullet;
    }

    private void OnTakeFromPool(PlayerBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnToPool(PlayerBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(PlayerBullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}