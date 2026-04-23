using UnityEngine;
using UnityEngine.Pool;

public class EnemyHitExplotionPool : MonoBehaviour
{
    [SerializeField] private EnemyHitExplotion explosionPrefab;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 40;
    [SerializeField] private bool collectionCheck = true;

    private ObjectPool<EnemyHitExplotion> pool;

    public ObjectPool<EnemyHitExplotion> Pool => pool;

    private void Awake()
    {
        pool = new ObjectPool<EnemyHitExplotion>(
            CreateExplosion,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            collectionCheck,
            defaultCapacity,
            maxSize
        );
    }

    private EnemyHitExplotion CreateExplosion()
    {
        EnemyHitExplotion explosion = Instantiate(explosionPrefab, transform);
        explosion.gameObject.SetActive(false);
        explosion.SetPool(pool);
        return explosion;
    }

    private void OnTakeFromPool(EnemyHitExplotion explosion)
    {
        explosion.gameObject.SetActive(true);
    }

    private void OnReturnToPool(EnemyHitExplotion explosion)
    {
        explosion.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(EnemyHitExplotion explosion)
    {
        Destroy(explosion.gameObject);
    }
}