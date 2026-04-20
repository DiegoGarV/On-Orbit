using UnityEngine;

public class ExplosionAutoDestroy : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.5f;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}