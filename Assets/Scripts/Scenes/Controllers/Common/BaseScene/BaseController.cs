using UnityEngine;
using TMPro;

public abstract class BaseLevelController : MonoBehaviour
{
    [Header("Panels UI")]
    // Panel que es mostra quan el joc està en pausa.
    public GameObject PausePanel;
    
    // Panel que es mostra quan el jugador completa el nivell amb èxit.
    public GameObject VictoryPanel;
    
    // Panel que es mostra quan el jugador falla el nivell.
    public GameObject DefeatPanel;
    
    // Propietats abstractes que ha d'implementar cada nivell.
    public abstract float RemainingTime { get; set; }
    public abstract bool VictoryConditionAchieved { get; }
    public abstract bool DefeatConditionAchieved { get; }

    // Retorna el temps límit del nivell.
    public abstract float LimitTime { get; }

    // Mètodes abstractes que cada nivell ha d'implementar.
    protected abstract void Start(); 
    protected abstract void LevelInitialize(); 
    public abstract void HUDUpdate();
    public abstract void LogicLevelUpdate();
    public abstract void VictoryPanelShow();
    public abstract void DefeatPanelShow();
    public virtual void OnItemCollected(string itemType, int quantity){}

    // Mètodes comuns ·······················

    // Gestiona el temporitzador del joc.
    // S'assegura que el temps baixi progressivament i no sigui negatiu.
    public virtual void TimeManager()
    {
        RemainingTime -= Time.deltaTime;
        if (RemainingTime <= 0)
        {
            RemainingTime = 0f;
        }
    }

    // Mostra el panel de pausa.
    public void ShowPausePanel()
    {
        HidePanels();
        PausePanel?.SetActive(true);
        ShowCursor();
    }

    // Amaga el panel de pausa.
    public void HidePausePanel()
    {
        PausePanel?.SetActive(false);
        HideCursor();
    }

    // Amaga tots els panells.
    public void HidePanels()
    {
        PausePanel?.SetActive(false);
        VictoryPanel?.SetActive(false);
        DefeatPanel?.SetActive(false);
    }

    // Botons comuns ----------------------------------

    // Serveix per reprendre el joc després d'una pausa o menú de pausa.
    public void GameContinue() { GameManager.gameInstance?.GameResume(); }

    // Serveix per reiniciar el nivell actual, tornant a l'inici del mateix.
    public void LevelRetry() { GameManager.gameInstance?.LevelCurrentRetry(); }

    // Serveix per carregar una escena específica, indicada per l'índex.
    public void SceneLoad(int sceneIndex) { GameManager.gameInstance?.SceneLoad(sceneIndex); }

    // Serveix per sortir del joc (tancar l'aplicació).
    public void GameExit() { GameManager.gameInstance?.GameExit(); }
    
    // -------------------------------------------------------

    // Mostra el cursor del ratolí.
    protected void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Amaga el cursor del ratolí.
    protected void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Converteix un temps en segons a un format de minuts i segons.
    protected string TimeFormat(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}