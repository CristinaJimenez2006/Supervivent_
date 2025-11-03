using UnityEngine;

// Controla el comportament d'una llanterna en el joc.
// Permet que el jugador reculli la llanterna en col·lidir amb ella, actualitzant l'inventari
// i reproduint un so de recol·lecció.
public class Recollectible : MonoBehaviour
{
    [Header("Configuration recollectible")]
    // Etiqueta per identificar el jugador en les col·lisions.
    [SerializeField] private string tagPlayer = "Player";

    // Tipus d'objecte que es recull (per exemple: "Recollectible", "Key", etc.).
    [SerializeField] private string itemType = "Recollectible";

    // Quantitat d'objectes que s'afegeixen a l'inventari quan es recull.
    [SerializeField] private int quantity = 1;

    // Mètode que es crida automàticament quan un altre objecte entra en el Collider d'aquest objecte
    // marcat com a "Trigger".
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si l'objecte que ha col·lidit és el jugador.
        if (other.CompareTag(tagPlayer))
        {
            // Notificar al GameManager que s'ha recollit un objecte.
            if (GameManager.gameInstance != null)
            {
                GameManager.gameInstance.CollectItem(itemType, quantity);
            }

            // Reprodueix el so de recol·lecció si l'AudioManager està disponible.
            AudioManager.audioInstance?.PlaySoundCollect();

            // Destruir l'objecte després de ser recollit.
            Destroy(gameObject);
        }
    }
}