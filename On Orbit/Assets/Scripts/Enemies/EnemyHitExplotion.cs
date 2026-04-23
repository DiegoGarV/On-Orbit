using UnityEngine;
using UnityEngine.Pool;

public class EnemyHitExplotion : MonoBehaviour
{
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private Animator animator;

    private IObjectPool<EnemyHitExplotion> pool;
    private bool isPlaying = false;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void SetPool(IObjectPool<EnemyHitExplotion> explosionPool)
    {
        pool = explosionPool;
    }

    public void PlayAt(Vector3 position)
    {
        transform.position = position;
        isPlaying = true;

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        CancelInvoke();
        Invoke(nameof(ReturnToPool), duration);
    }

    private void ReturnToPool()
    {
        if (!isPlaying) return;

        isPlaying = false;
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