using TMPro;  
using UnityEngine;

// S'encarrega de gestionar tota la lògica del nivell, incloent temps, ítems recol·lectables i condicions de victòria o derrota.
public class Level1Controller : BaseLevelController
{
    [Header("Configuration level 1")]
    // Textes per mostrar informació durant el joc i en els resultats.
    public TextMeshProUGUI recollectibleText;
    public TextMeshProUGUI recollectibleTextResultVictory;
    public TextMeshProUGUI recollectibleTextResultDefeat;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeTextResultVictory;
    public TextMeshProUGUI timeTextResultDefeat;

    // Nombre d'objectes recol·lectats necessaris per guanyar el nivell.
    [SerializeField] private int recollectiblesNecessary = 5;

    // Temps límit del nivell en segons.
    [SerializeField] private float limitTimeLevel1 = 180f;
    public override float LimitTime { get { return limitTimeLevel1; } }

    // Comptador d'objectes recol·lectats durant el nivell.
    private int recollectiblesCollectedLevel1 = 0;

    // Nom de l'ítem que es pot recollir.
    [SerializeField] private string nameRecollectible = "Recollectible";
    
    // Temps restant per completar el nivell.
    private float remainingTimeLevel1 = 0f;

    // Propietat per obtenir i modificar el temps restant.
    public override float RemainingTime
    {
        get { return remainingTimeLevel1; }
        set { remainingTimeLevel1 = value; }
    }

    // Condició de victòria: si s'han recollit tots els objectes recol·lectats.
    public override bool VictoryConditionAchieved { get { return recollectiblesCollectedLevel1 >= recollectiblesNecessary; } }

    // Condició de derrota: si el temps s'ha esgotat.
    public override bool DefeatConditionAchieved { get { return remainingTimeLevel1 <= 0; } }

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
        recollectiblesCollectedLevel1 = 0;
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
        // Actualitza el comptador d'objectes recol·lectats a la pantalla.
        RecollectiblesUpdate();

        // Actualitza el temps a la pantalla.
        TimeUpdate();
    }

    // Mètode cridat quan es recull un ítem.
    public override void OnItemCollected(string itemType, int quantity)
    {
        // Si l'ítem recollit és un objecte recol·lectat.
        if (itemType == nameRecollectible)
        {
            // Augmenta el comptador d'objectes recol·lectats.
            recollectiblesCollectedLevel1 += quantity;
            RecollectiblesUpdate();
        }
    }

    // Actualitza el comptador d'objectes recol·lectats al HUD.
    private void RecollectiblesUpdate()
    {
        if (recollectibleText != null) recollectibleText.text = $"{recollectiblesCollectedLevel1} / {recollectiblesNecessary}";
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
        ResultsUpdate(recollectibleTextResultVictory, timeTextResultVictory);

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
        ResultsUpdate(recollectibleTextResultDefeat, timeTextResultDefeat);

        // Reprodueix so de derrota.
        AudioManager.audioInstance?.PlaySoundDefeat();
    }

    // Actualitza els textos dels objectes recol·lectats i temps en l'escena de resultats.
    private void ResultsUpdate(TextMeshProUGUI recollectibleTextResult, TextMeshProUGUI timeTextResult)
    {
        if (recollectibleTextResult != null)
            recollectibleTextResult.text = $"{recollectiblesCollectedLevel1}/{recollectiblesNecessary}";

        if (timeTextResult != null)
        {
            // Calcula el temps utilitzat.
            float playerTime = LimitTime - RemainingTime;
            timeTextResult.text = TimeFormat(playerTime);
        }
    }
}