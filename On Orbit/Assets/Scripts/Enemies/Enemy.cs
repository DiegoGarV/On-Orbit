using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;

    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject enemyBulletPrefab;

    [Header("Entry Movement")]
    [SerializeField] private Vector3 localBattleOffset = Vector3.zero;
    [SerializeField] private float entrySpeed = 4f;

    [Header("Formation Movement")]
    [SerializeField] private float horizontalAmplitude = 1.2f;
    [SerializeField] private float horizontalSpeed = 1.5f;

    [Header("Attack Pattern")]
    [SerializeField] private float patternStartDelay = 0f;
    [SerializeField] private float burstCooldown = 1.5f;
    [SerializeField] private int shotsPerBurst = 3;
    [SerializeField] private float timeBetweenShots = 0.15f;

    private int currentHealth;
    private Vector3 startBattlePosition;
    private bool inBattlePosition = false;
    private bool canOscillate = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        startBattlePosition = transform.position + localBattleOffset;
    }

    private void Start()
    {
        StartCoroutine(BattleRoutine());
    }

    private void Update()
    {
        if (!inBattlePosition || !canOscillate)
            return;

        float offsetX = Mathf.PingPong(Time.time * horizontalSpeed, horizontalAmplitude * 2f) - horizontalAmplitude;

        Vector3 newPosition = startBattlePosition + new Vector3(offsetX, 0f, 0f);
        transform.position = newPosition;
    }

    private IEnumerator BattleRoutine()
    {
        if (patternStartDelay > 0f)
            yield return new WaitForSeconds(patternStartDelay);

        while (Vector3.Distance(transform.position, startBattlePosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                startBattlePosition,
                entrySpeed * Time.deltaTime
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
            yield return new WaitForSeconds(burstCooldown);
        }
    }

    private IEnumerator FireBurst()
    {
        if (firePoint == null || enemyBulletPrefab == null)
            yield break;

        for (int i = 0; i < shotsPerBurst; i++)
        {
            Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenShots);
        }
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
        Vector3 battlePosition = transform.position + localBattleOffset;

        // Posición de batalla
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(battlePosition, 0.15f);

        // Movimiento horizontal
        Vector3 leftPoint = battlePosition + Vector3.left * horizontalAmplitude;
        Vector3 rightPoint = battlePosition + Vector3.right * horizontalAmplitude;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawWireSphere(leftPoint, 0.08f);
        Gizmos.DrawWireSphere(rightPoint, 0.08f);
    }
}