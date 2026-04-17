using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager uiManager;
    private bool hasWon = false;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }

    private void Update()
    {
        if (hasWon) return;

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        if (enemies.Length == 0)
        {
            hasWon = true;

            if (uiManager != null)
            {
                uiManager.HandlePlayerVictory();
            }
        }
    }
}