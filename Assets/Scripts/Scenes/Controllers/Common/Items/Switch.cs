using UnityEngine;
using UnityEngine.UI;

// Permet que un jugador en el joc encengui o apagui una llum quan estigui a prop de l'interruptor 
// i premi una tecla específica.
public class Switch : MonoBehaviour
{
    [Header("Configuration")]
    // La llum que aquest interruptor controlarà.
    public Light lightToControl;

    // Tecla utilitzada per interactuar amb l'interruptor.
    public KeyCode interactKey = KeyCode.E;

    // Etiqueta per identificar el jugador en les col·lisions.
    [SerializeField] private string tagPlayer = "Player";

    [Header("UI")]
    // Missatge que es mostrarà quan el jugador estigui a prop.
    public GameObject interactMessage;
    
    // Indica si el jugador està a prop de l'interruptor.
    private bool playerNearby = false;
    
    // Estat actual de la llum: 'true' si està encesa, 'false' si està apagada.
    private bool isOn = false;

    void Start()
    {
        // Assegura que la llum comenci apagada en iniciar el joc.
        if (lightToControl != null)
        {
            lightToControl.enabled = false;
        }

        // Amaga el missatge d'interacció al inici.
        if (interactMessage != null)
        {
            interactMessage.SetActive(false);
        }
    }

    void Update()
    {
        // Si el jugador està a prop i prem la tecla d'interacció, canvia l'estat de la llum.
        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            ToggleState();
        }
    }

    // Canvia l'estat de la llum (encesa/apagada).
    private void ToggleState()
    {
        // Inverteix el valor booleà de l'estat.
        isOn = !isOn;

        // Actualitza l'estat de la llum si existeix la referència.
        if (lightToControl != null)
        {
            lightToControl.enabled = isOn;
            
            // Reprodueix el so de l'interruptor de llum si l'AudioManager està disponible.
            AudioManager.audioInstance?.PlayLightSwitchSound();
        }
    }

    // Detecta quan un collider entra en l'àrea d'activació (trigger).
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si l'objecte que entra és el jugador.
        if (other.CompareTag(tagPlayer))
        {
            playerNearby = true;

            // Mostra el missatge d'interacció.
            if (interactMessage != null)
            {
                interactMessage.SetActive(true);
            }
        }
    }

    // Detecta quan un collider surt de l'àrea d'activació (trigger).
    private void OnTriggerExit(Collider other)
    {
        // Verifica si l'objecte que surt és el jugador.
        if (other.CompareTag(tagPlayer))
        {
            playerNearby = false;

            // Amaga el missatge d'interacció.
            if (interactMessage != null)
            {
                interactMessage.SetActive(false);
            }
        }
    }
}