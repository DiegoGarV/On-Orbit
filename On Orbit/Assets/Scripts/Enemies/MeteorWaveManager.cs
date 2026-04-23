using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorWaveManager : MonoBehaviour
{
    public enum SpawnSide
    {
        Left,
        Right
    }

    [System.Serializable]
    public class MeteorWave
    {
        public float delay = 1f;
        public SpawnSide side = SpawnSide.Left;
        public List<int> rows = new List<int>();
        public float meteorSpeed = 6f;
        public float alertDuration = 1f;
    }

    [Header("Prefabs")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject alertPrefab;

    [Header("Lane Setup")]
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private int totalRows = 10;
    [SerializeField] private float rowSpacing = 1f;
    [SerializeField] private int centerRowIndex = 4;

    [Header("Waves")]
    [SerializeField] private List<MeteorWave> waves = new List<MeteorWave>();

    private AudioManager audioManager;
    
    private void Awake()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    
    private void Start()
    {
        foreach (MeteorWave wave in waves)
        {
            StartCoroutine(RunSingleWave(wave));
        }
    }

    private IEnumerator RunSingleWave(MeteorWave wave)
    {
        yield return new WaitForSeconds(wave.delay);
        yield return StartCoroutine(SpawnWave(wave));
    }

    private IEnumerator RunWaves()
    {
        foreach (MeteorWave wave in waves)
        {
            yield return new WaitForSeconds(wave.delay);
            StartCoroutine(SpawnWave(wave));
        }
    }

    private IEnumerator SpawnWave(MeteorWave wave)
    {
        SpawnAlerts(wave);

        yield return new WaitForSeconds(wave.alertDuration);

        foreach (int row in wave.rows)
        {
            SpawnMeteor(wave.side, row, wave.meteorSpeed);
        }
    }

    private void SpawnAlerts(MeteorWave wave)
    {
        if (alertPrefab == null) return;

        foreach (int row in wave.rows)
        {
            Vector3 spawnPosition = GetSpawnPosition(wave.side, row);
            GameObject alert = Instantiate(alertPrefab, spawnPosition, Quaternion.identity);
            Destroy(alert, wave.alertDuration);
        }
    }

    private void SpawnMeteor(SpawnSide side, int row, float speed)
    {
        if (meteorPrefab == null) return;

        Vector3 spawnPosition = GetSpawnPosition(side, row);
        GameObject meteorObject = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

        MeteorObstacle meteor = meteorObject.GetComponent<MeteorObstacle>();
        if (meteor == null) return;

        bool moveRight = side == SpawnSide.Left;
        meteor.Initialize(moveRight, speed);
    }

    private Vector3 GetSpawnPosition(SpawnSide side, int row)
    {
        Transform basePoint = side == SpawnSide.Left ? leftSpawnPoint : rightSpawnPoint;

        int clampedRow = Mathf.Clamp(row, 0, totalRows - 1);
        float yOffset = (centerRowIndex - clampedRow) * rowSpacing;

        return basePoint.position + new Vector3(0f, yOffset, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        if (leftSpawnPoint != null)
        {
            DrawLaneGizmos(leftSpawnPoint.position, Color.cyan);
        }

        if (rightSpawnPoint != null)
        {
            DrawLaneGizmos(rightSpawnPoint.position, Color.magenta);
        }
    }

    private void DrawLaneGizmos(Vector3 basePosition, Color color)
    {
        Gizmos.color = color;

        for (int row = 0; row < totalRows; row++)
        {
            float yOffset = (centerRowIndex - row) * rowSpacing;
            Vector3 lanePosition = basePosition + new Vector3(0f, yOffset, 0f);

            Gizmos.DrawWireSphere(lanePosition, 0.15f);
        }
    }
}