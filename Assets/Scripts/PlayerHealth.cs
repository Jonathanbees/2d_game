using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Respawn Settings")]
    public float respawnDelay = 0f;
    private bool isDead = false;

    [Header("Fall Detection")]
    public float fallLimit = -10f; // Posición Y donde se considera que cayó
    public bool checkFallOutOfScreen = true;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Verificar si el jugador cayó fuera de la pantalla
        if (checkFallOutOfScreen && transform.position.y < fallLimit)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto tiene la etiqueta "Danger"
        if (collision.gameObject.CompareTag("Danger"))
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // También detecta si es un Trigger
        if (other.CompareTag("Danger"))
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Evitar múltiples llamadas
        
        isDead = true;
        Debug.Log("¡Player murió! Reiniciando nivel...");
        
        // Reiniciar el nivel después del delay
        Invoke("RestartLevel", respawnDelay);
    }

    void RestartLevel()
    {
        // Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Método opcional para recibir daño (por si quieres usarlo después)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player tomó {damage} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}