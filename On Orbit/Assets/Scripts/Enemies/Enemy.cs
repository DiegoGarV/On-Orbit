using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private EnemyData enemyData;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform centerFirePoint;
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private EnemyBulletPool enemyBulletPool;

    private int currentHealth;
    private Vector3 startBattlePosition;
    private bool inBattlePosition = false;
    private bool canOscillate = false;

    private void Awake()
    {
        if (enemyData == null)
        {
            Debug.LogError($"EnemyData no está asignado en {gameObject.name}");
            return;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        ApplyData();
    }

    private void Start()
    {
        if (enemyData == null)
            return;

        StartCoroutine(BattleRoutine());
    }

    private void Update()
    {
        if (enemyData == null || !inBattlePosition || !canOscillate)
            return;

        float offsetX = Mathf.PingPong(
            Time.time * enemyData.horizontalSpeed,
            enemyData.horizontalAmplitude * 2f
        ) - enemyData.horizontalAmplitude;

        Vector3 newPosition = startBattlePosition + new Vector3(offsetX, 0f, 0f);
        transform.position = newPosition;
    }

    private void ApplyData()
    {
        currentHealth = enemyData.maxHealth;
        startBattlePosition = transform.position + enemyData.localBattleOffset;

        if (spriteRenderer != null && enemyData.enemySprite != null)
        {
            spriteRenderer.sprite = enemyData.enemySprite;
        }
    }

    private IEnumerator BattleRoutine()
    {
        if (enemyData.patternStartDelay > 0f)
            yield return new WaitForSeconds(enemyData.patternStartDelay);

        while (Vector3.Distance(transform.position, startBattlePosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                startBattlePosition,
                enemyData.entrySpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = startBattlePosition;
        inBattlePosition = true;

        yield return new WaitForSeconds(0.4f);

        canOscillate = true;

        while (true)
        {
            yield return StartCoroutine(FireBurst());
            yield return new WaitForSeconds(enemyData.burstCooldown);
        }
    }

    private IEnumerator FireBurst()
    {
        if (enemyBulletPool == null)
            yield break;

        for (int i = 0; i < enemyData.shotsPerBurst; i++)
        {
            FireShot();
            yield return new WaitForSeconds(enemyData.timeBetweenShots);
        }
    }

    private void FireShot()
    {
        switch (enemyData.shotType)
        {
            case EnemyShotType.Single:
                if (centerFirePoint != null)
                {
                    SpawnBullet(centerFirePoint);
                }
                break;

            case EnemyShotType.Double:
                if (leftFirePoint != null)
                {
                    SpawnBullet(leftFirePoint);
                }

                if (rightFirePoint != null)
                {
                    SpawnBullet(rightFirePoint);
                }
                break;
        }
    }

    private void SpawnBullet(Transform firePoint)
    {
        EnemyBullet bullet = enemyBulletPool.Pool.Get();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.Launch(enemyData, Vector2.down);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        Debug.Log($"Enemigo recibió {damageAmount} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyData == null)
            return;

        Vector3 battlePosition = transform.position + enemyData.localBattleOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(battlePosition, 0.15f);

        Vector3 leftPoint = battlePosition + Vector3.left * enemyData.horizontalAmplitude;
        Vector3 rightPoint = battlePosition + Vector3.right * enemyData.horizontalAmplitude;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawWireSphere(leftPoint, 0.08f);
        Gizmos.DrawWireSphere(rightPoint, 0.08f);
    }
}