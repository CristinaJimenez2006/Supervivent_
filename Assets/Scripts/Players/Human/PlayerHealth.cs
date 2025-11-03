using UnityEngine;

// Gestiona la vida del jugador, permetent rebre dany i curar-se.
// S'assegura que la vida mai excedeixi el màxim ni sigui menor a zero.
public class PlayerHealth : MonoBehaviour
{
    [Header("Life of the player")]
    // Vida màxima que pot tenir el jugador.
    public float maxHealth = 100f;
    
    // Vida actual del jugador en aquest moment.
    public float currentHealth;

    private void Start()
    {
        // Inicialitzar la vida al valor màxim quan comença el joc.
        currentHealth = maxHealth;
    }

    // Es crida quan el jugador rep dany.
    // Math.Clamp s'assegura que la vida no sigui menor a 0 ni major que la màxima.
    public void TakeDamage(float amount)
    {
        // Restar la quantitat de dany a la vida actual.
        currentHealth -= amount;
        
        // Assegurar-se que la vida estigui dins dels límits permessos.
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    // Es crida per curar al jugador.
    // Math.Clamp s'assegura que la vida no sigui menor a 0 ni major que la màxima.
    public void RestoreLive(float amount)
    {
        // Sumar la quantitat de curació a la vida actual.
        currentHealth += amount;
        
        // Assegurar-se que la vida estigui dins dels límits permessos.
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
}