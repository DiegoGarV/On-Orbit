using UnityEngine;

public enum EnemyShotType
{
    Single,
    Double
}

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Visual")]
    public Sprite enemySprite;
    public Sprite bulletSprite;

    [Header("Stats")]
    public int maxHealth = 100;
    public int bulletDamage = 10;
    public float bulletSpeed = 8f;
    public float bulletLifeTime = 5f;

    [Header("Movement - Entry")]
    public Vector3 localBattleOffset = Vector3.zero;
    public float entrySpeed = 4f;

    [Header("Movement - Formation")]
    public float horizontalAmplitude = 1.2f;
    public float horizontalSpeed = 1.5f;

    [Header("Attack Pattern")]
    public float patternStartDelay = 0f;
    public float burstCooldown = 1.5f;
    public int shotsPerBurst = 3;
    public float timeBetweenShots = 0.15f;

    [Header("Shooting Setup")]
    public EnemyShotType shotType = EnemyShotType.Single;
}