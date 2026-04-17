using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIController uiController;
    private bool hasWon = false;

    private void Awake()
    {
        uiController = FindFirstObjectByType<UIController>();
    }

    private void Update()
    {
        if (hasWon) return;

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        if (enemies.Length == 0)
        {
            hasWon = true;

            if (uiController != null)
            {
                uiController.HandlePlayerVictory();
            }
        }
    }
}