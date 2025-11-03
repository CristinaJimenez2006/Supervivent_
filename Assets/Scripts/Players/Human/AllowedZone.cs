using UnityEngine;

// Limita al jugador dins d'una zona específica definida per un BoxCollider i el retorna automàticament a l'àrea permesa si intenta sortir.
public class AllowedZone : MonoBehaviour
{
    // Àrea permesa on el jugador pot moure's, definida per un BoxCollider.
    public BoxCollider allowedArea;
    
    // Component CharacterController del jugador per controlar el seu moviment.
    private CharacterController characterController;

    void Start()
    {
        // Obtenir el component CharacterController al iniciar.
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Sortir si no hi ha àrea permesa definida.
        if (allowedArea == null) return;

        // Obtenir els límits de la zona permesa.
        Bounds limits = allowedArea.bounds;
        Vector3 positionPlayer = transform.position;

        // Verificar si el jugador està fora dels límits de la zona permesa.
        if (!limits.Contains(positionPlayer))
        {
            // Portar-lo de tornada al punt més proper dins de la zona permesa.
            Vector3 positionCorrect = limits.ClosestPoint(positionPlayer);

            // Moure el jugador de tornada a la posició correcta.
            // Es desactiva i s'activa el CharacterController per evitar conflictes amb la física.
            characterController.enabled = false;
            transform.position = positionCorrect;
            characterController.enabled = true;
        }
    }
}