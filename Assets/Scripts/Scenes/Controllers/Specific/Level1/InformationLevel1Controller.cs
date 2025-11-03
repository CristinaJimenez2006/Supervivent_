using UnityEngine;

// Controla el botó del panell d'informació ('Continuar').
// Aquest script es col·loca en un GameObject independent en l'escena d'informació del nivell, per evitar que els botons perdin la referència al Singleton.
public class InformationLevel1Controller : MonoBehaviour
{
    // Mètode que es crida quan es prem el botó "Continuar".
    public void StartPlaying()
    {
        // Carrega l'escena del nivell 1 mitjançant el GameManager.
        GameManager.gameInstance.SceneLoad((int)GameManager.GameScene.LEVEL_1);
    }
}