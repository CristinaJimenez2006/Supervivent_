using UnityEngine;
using UnityEngine.UI;

// Controla els botons d'opcions.
// Aquest script es col·loca en un GameObject independent en l'escena d'opcions, per evitar que els botons perdin la referència al Singleton.
public class OptionsController : MonoBehaviour
{
    [Header("Level buttons")]
    // Botó per accedir al nivell 1.
    public Button level1Button;
    // Botó per accedir al nivell 2.
    public Button level2Button;

    private void Start()
    { 
        // Obtenir el nivell més alt desbloquejat guardat a PlayerPrefs.
        int levelReached = PlayerPrefs.GetInt("levelReached", (int) GameManager.GameScene.INFORMATION_LEVEL1);
    
        // El nivell 1 sempre està desbloquejat.
        if (level1Button != null) level1Button.interactable = true;

        // El nivell 2 només es desbloqueja si es completa el nivell 1.
        if (level2Button != null) level2Button.interactable = levelReached > (int)GameManager.GameScene.INFORMATION_LEVEL2;
    }
    
    public void Level1Load()
    {
        // Carrega l'escena del nivell 1.
        GameManager.gameInstance.SceneLoad((int)GameManager.GameScene.INFORMATION_LEVEL1);
    }

    public void Level2Load()
    {
        // Carrega l'escena del nivell 2.
        GameManager.gameInstance.SceneLoad((int)GameManager.GameScene.INFORMATION_LEVEL2);
    }

    public void MainMenuReturn()
    {
        // Carrega l'escena del menú principal.
        GameManager.gameInstance.SceneLoad((int)GameManager.GameScene.MAIN_MENU);
    }
}