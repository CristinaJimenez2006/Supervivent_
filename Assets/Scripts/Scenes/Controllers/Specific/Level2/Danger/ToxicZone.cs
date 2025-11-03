using UnityEngine;

// És una "zona de dany" que aplica dany per segon al jugador quan entra en ella.
public class ToxicZone : MonoBehaviour
{
    [Header("Damage per second")]
    // Quantitat de dany per segon que aplica la zona tòxica.
    public float damagePerSecond = 10f;
    
    // Etiqueta per identificar el jugador en les col·lisions.
    [SerializeField] private string tagPlayer = "Player";

    // S'executa automàticament quan un altre collider està dins del trigger d'aquest GameObject.
    private void OnTriggerStay(Collider other)
    {
        // Verificar si l'objecte que està dins del trigger és el jugador.
        if (other.CompareTag(tagPlayer))
        {
            // Obtenir el component PlayerHealth del jugador.
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Si el jugador té component de salut, aplicar el dany progressiu.
            if (playerHealth != null)
            {
                // Dany progressiu mentre el jugador estigui dins de la zona.
                playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}