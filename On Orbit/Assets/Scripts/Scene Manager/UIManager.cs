using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private CanvasGroup winPanel;
    [SerializeField] private CanvasGroup losePanel;

    [Header("Player Control")]
    [SerializeField] private ActionManager playerActionManager;

    [Header("Lives UI")]
    [SerializeField] private Animator[] heartAnimators;
    [SerializeField] private float finalHeartAnimationDelay = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;

    private bool gameEnded = false;

    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }
    }

    private void Start()
    {
        HidePanel(winPanel);
        HidePanel(losePanel);
        ResetHearts();
    }

    public void InitializeLives(int maxLives)
    {
        ResetHearts();
    }

    public void OnPlayerLifeLost(int currentLives)
    {
        int lostHeartIndex = currentLives;

        if (lostHeartIndex >= 0 && lostHeartIndex < heartAnimators.Length)
        {
            Animator heartAnimator = heartAnimators[lostHeartIndex];

            if (heartAnimator != null)
            {
                heartAnimator.SetTrigger("LoseLife");
            }
        }
    }

    public void HandlePlayerDefeat()
    {
        if (gameEnded) return;
        StartCoroutine(HandlePlayerDefeatRoutine());
    }

    private IEnumerator HandlePlayerDefeatRoutine()
    {
        gameEnded = true;

        DisablePlayerInput();

        yield return new WaitForSeconds(finalHeartAnimationDelay);

        if (audioManager != null)
        {
            audioManager.PlayDefeatMusic();
        }

        ShowPanel(losePanel);
        HidePanel(winPanel);

        Time.timeScale = 0f;
    }

    public void HandlePlayerVictory()
    {
        if (gameEnded) return;
        gameEnded = true;

        DisablePlayerInput();

        if (audioManager != null)
        {
            audioManager.PlayVictoryMusic();
        }

        ShowPanel(winPanel);
        HidePanel(losePanel);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (audioManager != null)
        {
            audioManager.PlayLevelMusic();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void DisablePlayerInput()
    {
        if (playerActionManager != null)
        {
            playerActionManager.enabled = false;
        }
    }

    private void ShowPanel(CanvasGroup panel)
    {
        if (panel == null) return;

        panel.alpha = 1f;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }

    private void HidePanel(CanvasGroup panel)
    {
        if (panel == null) return;

        panel.alpha = 0f;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }

    private void ResetHearts()
    {
        for (int i = 0; i < heartAnimators.Length; i++)
        {
            if (heartAnimators[i] != null)
            {
                heartAnimators[i].Rebind();
                heartAnimators[i].Update(0f);
            }
        }
    }
}