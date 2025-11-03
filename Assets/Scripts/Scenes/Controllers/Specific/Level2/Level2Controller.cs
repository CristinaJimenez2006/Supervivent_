using TMPro;
using UnityEngine;

// S'encarrega de gestionar tota la lògica del nivell, incloent temps, vida del jugador i condicions de victòria o derrota.
public class Level2Controller : BaseLevelController
{
    [Header("Configuration level 2")]
    // Textes per mostrar informació durant el joc i en els resultats.
    public TextMeshProUGUI liveText;
    public TextMeshProUGUI liveTextResultVictory;
    public TextMeshProUGUI liveTextResultDefeat;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeTextResultVictory;
    public TextMeshProUGUI timeTextResultDefeat;
    
    // Referència a la salut del jugador.
    public PlayerHealth playerHealth;

    // Temps límit del nivell en segons.
    [SerializeField] private float limitTimeLevel2 = 180f;
    public override float LimitTime { get { return limitTimeLevel2; } }

    // Temps restant per completar el nivell.
    private float remainingTimeLevel2 = 0f;

    // Propietat per obtenir i modificar el temps restant.
    public override float RemainingTime
    {
        get { return remainingTimeLevel2; }
        set { remainingTimeLevel2 = value; }
    }

    // Condició de victòria: quan el temps s'acaba, si el jugador ha sobreviscut.
    public override bool VictoryConditionAchieved { get { return remainingTimeLevel2 <= 0; } }

    // Condició de derrota: si el jugador ha mort (vida igual o inferior a zero).
    public override bool DefeatConditionAchieved { get { return playerHealth != null && playerHealth.currentHealth <= 0; } }

    // Mètode cridat a l'inici del nivell.
    protected override void Start()
    {
        LevelInitialize();
    }

    // Inicialitza variables i amaga panells.
    protected override void LevelInitialize()
    {
        if (GameManager.gameInstance == null) return;
        HidePanels();
        RemainingTime = LimitTime;
        
        // Buscar el component PlayerHealth si no està assignat.
        if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Actualitza la lògica del nivell cada frame.
    public override void LogicLevelUpdate()
    {
        // Actualitza el temps restant.
        TimeManager();

        // Comprova condicions de victòria o derrota.
        if (VictoryConditionAchieved)
        {
            // Notifica victòria al GameManager.
            GameManager.gameInstance.LevelComplete(true);
        }
        else if (DefeatConditionAchieved)
        {
            // Notifica derrota al GameManager.
            GameManager.gameInstance.LevelComplete(false);
        }
    }

    // Actualitza els elements del HUD cada frame.
    public override void HUDUpdate()
    {
        // Actualitza la vida del jugador a la pantalla.
        LiveUpdate();

        // Actualitza el temps a la pantalla.
        TimeUpdate();
    }

    // Actualitza la vida del jugador al HUD.
    private void LiveUpdate()
    {
        if (playerHealth != null && liveText != null) liveText.text = $"{Mathf.Round(playerHealth.currentHealth)}";
    }

    // Actualitza el temps al HUD.
    private void TimeUpdate()
    {
        // Formata el temps.
        if (timeText != null) timeText.text = TimeFormat(RemainingTime);
    }

    // Mostra el panell de victòria.
    public override void VictoryPanelShow()
    {
        HidePanels();
        VictoryPanel.SetActive(true);
        ShowCursor();

        // Actualitza resultats.
        ResultsUpdate(liveTextResultVictory, timeTextResultVictory);

        // Reprodueix so de victòria.
        AudioManager.audioInstance?.PlaySoundVictory();
    }

    // Mostra el panell de derrota.
    public override void DefeatPanelShow()
    {
        HidePanels();
        DefeatPanel.SetActive(true);
        ShowCursor();

        // Actualitza resultats.
        ResultsUpdate(liveTextResultDefeat, timeTextResultDefeat);

        // Reprodueix so de derrota.
        AudioManager.audioInstance?.PlaySoundDefeat();
    }

    // Actualitza els textos de vida i temps en l'escena de resultats.
    private void ResultsUpdate(TextMeshProUGUI liveTextResult, TextMeshProUGUI timeTextResult)
    {
        if (liveTextResult != null && playerHealth != null)
            liveTextResult.text = $"{liveText.text}";

        if (timeTextResult != null)
        {
            // Calcula el temps utilitzat.
            float playerTime = LimitTime - RemainingTime;
            timeTextResult.text = TimeFormat(playerTime);
        }
    }
}