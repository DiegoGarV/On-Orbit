using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private int defaultCapacity = 30;
    [SerializeField] private int maxSize = 150;
    [SerializeField] private bool collectionCheck = true;

    private ObjectPool<EnemyBullet> pool;

    public ObjectPool<EnemyBullet> Pool => pool;

    private void Awake()
    {
        pool = new ObjectPool<EnemyBullet>(
            CreateBullet,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            collectionCheck,
            defaultCapacity,
            maxSize
        );
    }

    private EnemyBullet CreateBullet()
    {
        EnemyBullet bullet = Instantiate(bulletPrefab, transform);
        bullet.gameObject.SetActive(false);
        bullet.SetPool(pool);
        return bullet;
    }

    private void OnTakeFromPool(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnToPool(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}