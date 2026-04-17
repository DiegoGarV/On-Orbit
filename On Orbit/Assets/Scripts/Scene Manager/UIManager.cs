using UnityEngine;
using UnityEngine.SceneManagement;
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

    private bool gameEnded = false;

    private void Start()
    {
        HidePanel(winPanel);
        HidePanel(losePanel);
    }

    public void HandlePlayerDefeat()
    {
        if (gameEnded) return;
        gameEnded = true;

        DisablePlayerInput();
        ShowPanel(losePanel);
        HidePanel(winPanel);

        Time.timeScale = 0f;
    }

    public void HandlePlayerVictory()
    {
        if (gameEnded) return;
        gameEnded = true;

        DisablePlayerInput();
        ShowPanel(winPanel);
        HidePanel(losePanel);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
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
}