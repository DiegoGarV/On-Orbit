using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public enum SpawnSide
    {
        Left,
        Right
    }

    [Header("Setup")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject alertPrefab;

    [Header("Timing")]
    [SerializeField] private float spawnDelay = 3f;
    [SerializeField] private float alertDuration = 1f;

    [Header("Meteor Movement")]
    [SerializeField] private SpawnSide spawnSide = SpawnSide.Left;
    [SerializeField] private float meteorSpeed = 6f;

    [Header("Spawn Position")]
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;

    [Header("Gizmos")]
    [SerializeField] private float gizmoSphereRadius = 0.2f;
    [SerializeField] private float gizmoArrowLength = 1.2f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        if (spawnDelay > alertDuration)
        {
            yield return new WaitForSeconds(spawnDelay - alertDuration);
        }

        SpawnAlert();

        yield return new WaitForSeconds(alertDuration);

        SpawnMeteor();
    }

    private void SpawnAlert()
    {
        if (alertPrefab == null)
            return;

        GameObject alert = Instantiate(alertPrefab, transform.position + spawnOffset, Quaternion.identity);
        Destroy(alert, alertDuration);
    }

    private void SpawnMeteor()
    {
        if (meteorPrefab == null)
            return;

        GameObject meteorObject = Instantiate(meteorPrefab, transform.position + spawnOffset, Quaternion.identity);

        MeteorObstacle meteor = meteorObject.GetComponent<MeteorObstacle>();
        if (meteor == null)
            return;

        bool moveRight = spawnSide == SpawnSide.Left;
        meteor.Initialize(moveRight, meteorSpeed);
    }

    private void OnDrawGizmos()
    {
        Vector3 spawnPosition = transform.position + spawnOffset;
        Vector3 direction = spawnSide == SpawnSide.Left ? Vector3.right : Vector3.left;

        // Punto principal del spawn
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPosition, gizmoSphereRadius);

        // Línea de dirección
        Gizmos.color = Color.red;
        Gizmos.DrawLine(spawnPosition, spawnPosition + direction * gizmoArrowLength);

        // Punta simple de flecha
        Vector3 arrowTip = spawnPosition + direction * gizmoArrowLength;
        Vector3 sideA = arrowTip + Quaternion.Euler(0f, 0f, 150f) * (direction * 0.3f);
        Vector3 sideB = arrowTip + Quaternion.Euler(0f, 0f, -150f) * (direction * 0.3f);

        Gizmos.DrawLine(arrowTip, sideA);
        Gizmos.DrawLine(arrowTip, sideB);
    }
}