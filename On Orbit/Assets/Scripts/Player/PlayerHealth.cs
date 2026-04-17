using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private UIManager uiManager;

    private void Awake()
    {
        currentHealth = maxHealth;
        uiManager = FindFirstObjectByType<UIManager>();
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
        if (uiManager != null)
        {
            uiManager.HandlePlayerDefeat();
        }

        gameObject.SetActive(false);
    }
}