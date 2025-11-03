using UnityEngine;
using System.Collections;

// Permet a un objecte (en aquest cas una porta) rotar suaument entre un estat obert i un estat tancat cada vegada
// que el jugador prem una tecla específica. També reprodueix un so en activar-se.
public class Door : MonoBehaviour
{
    // Defineix la tecla que s'utilitzarà per obrir/tancar la porta (KeyCode.F per defecte).
    public KeyCode toggleKey = KeyCode.F;

    // Angle de rotació de la porta quan està completament oberta.
    public float doorOpenAngle = 95f;

    // Angle de rotació de la porta quan està completament tancada.
    public float doorCloseAngle = 0.0f;

    // Controla la velocitat d'obertura/tancament.
    public float smooth = 3.0f;

    // Missatge que es mostrarà al jugador quan estigui a prop.
    public GameObject interactMessage;
    [SerializeField] private string tagPlayer = "Player";

    // Indica si el jugador està a prop de la porta.
    private bool playerNearby = false;

    // Estat booleà de la porta: 'true' si està oberta, 'false' si està tancada.
    private bool doorOpen = false;

    [Header("Collision Settings")]
    // Collider que bloqueja el pas físicament quan la porta està tancada.
    public Collider blockingCollider;
    
    // Trigger per detectar quan el jugador està a prop per interactuar.
    public Collider interactionTrigger;

    void Start()
    {
        // Obtenir tots els colliders del objecte.
        Collider[] colliders = GetComponents<Collider>();

        // Assignar automàticament els colliders segons si són triggers o no.
        foreach (Collider collider in colliders)
        {
            // Per detectar al jugador (és un trigger).
            if (collider.isTrigger) interactionTrigger = collider;
            // Per bloqueig físic (no és un trigger).
            else blockingCollider = collider;
        }

        // Amagar el missatge d'interacció al inici.
        if (interactMessage != null)
        {
            interactMessage.SetActive(false);
        }
    }

    void Update()
    {
        // Comprovar si el jugador està a prop i la tecla 'toggleKey' ha estat premuda.
        if (playerNearby && Input.GetKeyDown(toggleKey))
        {
            // Canvia l'estat actual de la porta (d'obert a tancat, o viceversa).
            doorOpen = !doorOpen;

            // Reprodueix el so de la porta si l'AudioManager està disponible.
            AudioManager.audioInstance?.PlayDoorSound();
        }

        // Rotar la porta suaument cap a la posició oberta o tancada segons l'estat actual.
        if (doorOpen)
        {
            // Defineix la rotació objectiu usant l'angle d'obertura (eix Y).
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);

            // Mou la rotació actual del objecte cap a la rotació objectiu de forma suau.
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        else
        {
            // Defineix la rotació objectiu usant l'angle de tancament.
            Quaternion targetRotation2 = Quaternion.Euler(0, doorCloseAngle, 0);

            // Mou la rotació actual cap a l'angle de tancament, de forma suau.
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);
        }
    }

    // Detecta quan un collider entra en l'àrea d'activació (trigger).
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si l'objecte que entra és el jugador.
        if (other.CompareTag(tagPlayer))
        {
            playerNearby = true;

            // Mostrar el missatge d'interacció.
            if (interactMessage != null)
            {
                interactMessage.SetActive(true);
            }
        }
    }

    // Detecta quan un collider surt de l'àrea d'activació (trigger).
    private void OnTriggerExit(Collider other)
    {
        // Verificar si l'objecte que surt és el jugador.
        if (other.CompareTag(tagPlayer))
        {
            playerNearby = false;

            // Amagar el missatge d'interacció.
            if (interactMessage != null)
            {
                interactMessage.SetActive(false);
            }
        }
    }
}