using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private EnemyData enemyData;

    [Header("Instance Setup")]
    [SerializeField] private Vector3 localBattleOffset = Vector3.zero;
    [SerializeField] private float patternStartDelay = 0f;

    [Range(0f, 1f)]
    [SerializeField] private float startPointOnPath = 0.5f;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Transform centerFirePoint;
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private EnemyBulletPool enemyBulletPool;

    private int currentHealth;

    private Vector3 pathCenterPosition;
    private Vector3 entryTargetPosition;

    private bool inBattlePosition = false;
    private bool canOscillate = false;

    private float oscillationTime = 0f;

    private AudioManager audioManager;

    private void Awake()
    {
        if (enemyData == null)
        {
            Debug.LogError($"EnemyData no está asignado en {gameObject.name}");
            return;
        }

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyCollider == null)
            enemyCollider = GetComponent<Collider2D>();

        audioManager = FindFirstObjectByType<AudioManager>();

        ApplyData();
        SetVisible(false);
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

        oscillationTime += Time.deltaTime * enemyData.horizontalSpeed;

        float normalizedPingPong = Mathf.PingPong(oscillationTime, 1f);
        float xOffset = Mathf.Lerp(
            -enemyData.horizontalAmplitude,
            enemyData.horizontalAmplitude,
            normalizedPingPong
        );

        Vector3 newPosition = pathCenterPosition + new Vector3(xOffset, 0f, 0f);
        transform.position = newPosition;
    }

    private void ApplyData()
    {
        currentHealth = enemyData.maxHealth;

        pathCenterPosition = transform.position + localBattleOffset;

        float startXOffset = Mathf.Lerp(
            -enemyData.horizontalAmplitude,
            enemyData.horizontalAmplitude,
            startPointOnPath
        );

        entryTargetPosition = pathCenterPosition + new Vector3(startXOffset, 0f, 0f);

        oscillationTime = startPointOnPath;

        if (spriteRenderer != null && enemyData.enemySprite != null)
        {
            spriteRenderer.sprite = enemyData.enemySprite;
        }
    }

    private IEnumerator BattleRoutine()
    {
        if (patternStartDelay > 0f)
            yield return new WaitForSeconds(patternStartDelay);

        SetVisible(true);

        while (Vector3.Distance(transform.position, entryTargetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                entryTargetPosition,
                enemyData.entrySpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = entryTargetPosition;
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
            if (audioManager != null)
            {
                audioManager.PlayEnemyShootSFX();
            }
            yield return new WaitForSeconds(enemyData.timeBetweenShots);
        }
    }

    private void FireShot()
    {
        switch (enemyData.shotType)
        {
            case EnemyShotType.Single:
                if (centerFirePoint != null)
                    SpawnBullet(centerFirePoint);
                break;

            case EnemyShotType.Double:
                if (leftFirePoint != null)
                    SpawnBullet(leftFirePoint);

                if (rightFirePoint != null)
                    SpawnBullet(rightFirePoint);
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

    private void SetVisible(bool visible)
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = visible;

        if (enemyCollider != null)
            enemyCollider.enabled = visible;
    }

    public void TakeDamage(int damageAmount)
    {
        if (spriteRenderer != null && !spriteRenderer.enabled)
            return;

        currentHealth -= damageAmount;

        if (audioManager != null)
        {
            audioManager.PlayEnemyHitSFX();
        }

        // Debug.Log($"Enemigo recibió {damageAmount} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (audioManager != null)
        {
            audioManager.PlayEnemyDeathSFX();
        }

        StopAllCoroutines();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyData == null)
            return;

        Vector3 pathCenter = transform.position + localBattleOffset;

        Vector3 leftPoint = pathCenter + Vector3.left * enemyData.horizontalAmplitude;
        Vector3 rightPoint = pathCenter + Vector3.right * enemyData.horizontalAmplitude;

        float startXOffset = Mathf.Lerp(
            -enemyData.horizontalAmplitude,
            enemyData.horizontalAmplitude,
            startPointOnPath
        );

        Vector3 redPoint = pathCenter + new Vector3(startXOffset, 0f, 0f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawWireSphere(leftPoint, 0.08f);
        Gizmos.DrawWireSphere(rightPoint, 0.08f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(redPoint, 0.15f);
    }
}