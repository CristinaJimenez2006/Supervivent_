using UnityEngine;

public class HealPickup : MonoBehaviour
{
    [Header("Amount of healing")]
    // Etiqueta per identificar el jugador en les col·lisions.
    [SerializeField] private string tagPlayer = "Player";

    // Quantitat de vida que curarà.
    [SerializeField] private float healAmount = 25f;

    // Mètode que es crida automàticament quan un altre objecte entra en el Collider d'aquest objecte
    // marcat com a "Trigger".
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si l'objecte que ha col·lidit és el jugador.
        if (other.CompareTag(tagPlayer))
        {
            // Obtenir el component PlayerHealth del jugador.
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Si el jugador té component de salut, aplicar la curació.
            if (playerHealth != null)
            {
                playerHealth.RestoreLive(healAmount);
            }

            // Reprodueix el so de recol·lecció si l'AudioManager està disponible.
            AudioManager.audioInstance?.PlaySoundCollect();

            // Destrueix l'objecte un cop recollit.
            Destroy(gameObject);
        }
    }
}