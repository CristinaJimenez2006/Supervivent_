using UnityEngine;

// Quan el jugador entra en aquesta zona, es teletransporta immediatament al punt de reaparició definit.
public class Crack : MonoBehaviour
{
    // Posició on reapareix el jugador.
    public Transform respawnPoint;

    // Etiqueta per identificar el jugador en les col·lisions.
    [SerializeField] private string tagPlayer = "Player";

    // S'executa automàticament quan un altre collider entra en el trigger d'aquest GameObject.
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si l'objecte que ha entrat és el jugador.
        if (other.CompareTag(tagPlayer))
        {
            // Obtenir el CharacterController del jugador (utilitzat per controlar el seu moviment).
            CharacterController characterController = other.GetComponent<CharacterController>();
            
            // Assegurar-se que hi ha CharacterController i punt de respawn abans de teletransportar.
            if (characterController != null && respawnPoint != null)
            {
                // Desactivar el CharacterController per evitar conflictes durant el teletransport.
                characterController.enabled = false;

                // Teletransportar el jugador a la posició del punt de reaparició.
                other.transform.position = respawnPoint.position;

                // Reactivar el CharacterController per permetre el moviment normal.
                characterController.enabled = true;
            }
        }
    }
}