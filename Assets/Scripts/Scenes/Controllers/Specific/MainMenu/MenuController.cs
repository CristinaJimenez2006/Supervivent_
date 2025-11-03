using UnityEngine;

// Controla els botons del menú principal.
// Aquest script es col·loca en un GameObject independent en l'escena del menú, per evitar que els botons perdin la referència al Singleton.
public class MenuController : MonoBehaviour
{
    public void LevelsOptionsOpen()
    {
        // Carrega l'escena d'opcions.
        GameManager.gameInstance.SceneLoad((int)GameManager.GameScene.OPTIONS);
    }

    public void ConfigurationOpen()
    {
        // Carrega l'escena de configuració.
        GameManager.gameInstance.SceneLoad((int)GameManager.GameScene.SETTINGS);
    }

    public void GameExit()
    {
        // Executa el mètode de sortir del joc.
        GameManager.gameInstance.GameExit();
    }
}