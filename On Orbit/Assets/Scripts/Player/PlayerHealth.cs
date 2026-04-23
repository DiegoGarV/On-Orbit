using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private int maxLives = 3;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityDuration = 1.2f;
    [SerializeField] private Animator playerAnimator;

    private int currentLives;
    private bool isInvulnerable = false;
    private bool isDead = false;

    private UIManager uiManager;
    private AudioManager audioManager;

    public int CurrentLives => currentLives;
    public bool IsInvulnerable => isInvulnerable;

    private void Awake()
    {
        currentLives = maxLives;
        uiManager = FindFirstObjectByType<UIManager>();
        audioManager = FindFirstObjectByType<AudioManager>();

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        if (uiManager != null)
        {
            uiManager.InitializeLives(maxLives);
        }
    }

    public bool TakeDamage(int damageAmount)
    {
        if (isDead || isInvulnerable)
            return false;

        currentLives--;
        currentLives = Mathf.Max(currentLives, 0);

        if (uiManager != null)
        {
            uiManager.OnPlayerLifeLost(currentLives);
        }

        if (currentLives <= 0)
        {
            Die();
            return true;
        }

        if (audioManager != null)
        {
            audioManager.PlayPlayerHitSFX();
        }

        StartCoroutine(InvulnerabilityRoutine());
        return true;
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        SetInvulnerabilityAnimation(true);

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false;
        SetInvulnerabilityAnimation(false);
    }

    private void SetInvulnerabilityAnimation(bool value)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsInvulnerable", value);
        }
    }

    private void Die()
    {
        isDead = true;
        isInvulnerable = true;
        SetInvulnerabilityAnimation(false);

        if (uiManager != null)
        {
            uiManager.HandlePlayerDefeat();
        }

        if (audioManager != null)
        {
            audioManager.PlayPlayerDeathSFX();
        }

        gameObject.SetActive(false);
    }
}