using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private UIController uiController;

    private void Awake()
    {
        currentHealth = maxHealth;
        uiController = FindFirstObjectByType<UIController>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        Debug.Log($"Jugador recibió {damageAmount} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (uiController != null)
        {
            uiController.HandlePlayerDefeat();
        }

        gameObject.SetActive(false);
    }
}