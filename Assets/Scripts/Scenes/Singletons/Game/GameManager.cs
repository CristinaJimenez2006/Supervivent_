using UnityEngine;
using UnityEngine.SceneManagement;

// Gestiona l'estat general del joc: escenes, pausa, HUD, nivells i Singletons.
public class GameManager : MonoBehaviour
{
    // Enumeració amb els possibles noms d'escenes del joc.
    public enum GameScene
    {
        MAIN_MENU = 0,
        OPTIONS = 1,
        SETTINGS = 2,
        INFORMATION_LEVEL1 = 3,
        LEVEL_1 = 4,
        INFORMATION_LEVEL2 = 5,
        LEVEL_2 = 6
    }

    // Instància única del GameManager per accedir-hi des de qualsevol lloc.
    public static GameManager gameInstance;

    [Header("Game States")]
    // Indica si el joc està actiu.
    public bool gameActived = false;

    // Indica si el joc està pausat.
    public bool gamePaused = false;

    // Referència al controlador del nivell actual.
    private BaseLevelController levelCurrentController;

    // Es crida en crear la instància, assegura el patró singleton.
    private void Awake()
    {
        SingletonManager();
    }

    // Es subscriu a l'esdeveniment de càrrega d'escena.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Es desubscriu de l'esdeveniment en desactivar l'objecte.
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Gestiona la instància única del GameManager.
    private void SingletonManager()
    {
        if (gameInstance != null && gameInstance != this)
            Destroy(gameObject);
        else
        {
            gameInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // S'executa cada vegada que es carrega una escena.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneInitialize();
    }

    // Inicialitza l'escena actual segons si és menú o nivell jugable.
    private void SceneInitialize()
    {
        // Cerca un controlador de nivell en l'escena.
        levelCurrentController = FindObjectOfType<BaseLevelController>();

        if (levelCurrentController != null)
            ConfigurePlayableLevel();
        else
            ConfigureMenu();

        // Reprodueix la música de fons.
        AudioManager.audioInstance?.PlayBackgroundMusic();
    }

    // Configura un nivell jugable.
    private void ConfigurePlayableLevel()
    {
        levelCurrentController.HidePanels();
        gameActived = true;
        gamePaused = false;
        Time.timeScale = 1f;
        HideCursor();
    }

    // Configura un menú principal o opcions.
    private void ConfigureMenu()
    {
        gameActived = false;
        gamePaused = false;
        Time.timeScale = 1f;
        ShowCursor();
    }

    // Es crida cada frame.
    private void Update()
    {
        PauseInputManager();
        LogicLevelUpdate();
    }

    // Gestiona l'entrada del botó 'Escape' per pausar o reprendre.
    private void PauseInputManager()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && levelCurrentController != null)
        {
            if (gamePaused) GameResume();
            else GamePaused();
        }
    }

    // Lògica d'actualització de nivell (HUD i lògica de joc).
    private void LogicLevelUpdate()
    {
        if (gameActived && !gamePaused)
        {
            levelCurrentController?.HUDUpdate();
            levelCurrentController?.LogicLevelUpdate();
        }
    }

    // Mètode cridat quan es recull un ítem.
    public void CollectItem(string itemType, int quantity)
    {
        if (!gameActived || gamePaused) return;

        levelCurrentController?.OnItemCollected(itemType, quantity);

        // Verifica condicions de victòria/derrota després de recollir l'ítem.
        CheckLevelStatus();
    }

    // Verifica si el nivell ha acabat en victòria o derrota.
    private void CheckLevelStatus()
    {
        if (levelCurrentController == null) return;

        if (levelCurrentController.VictoryConditionAchieved)
            LevelComplete(true);
        else if (levelCurrentController.DefeatConditionAchieved)
            LevelComplete(false);
    }

    // Finalitza el nivell mostrant panell de victòria o derrota.
    public void LevelComplete(bool victory)
    {
        if (!gameActived || levelCurrentController == null) return;

        gameActived = false;
        gamePaused = false;
        Time.timeScale = 0f;

        if (victory)
        {
            levelCurrentController.VictoryPanelShow();
            UnlockNextLevel();
        }
        else
        {
            levelCurrentController.DefeatPanelShow();
        }
    }

    // Desbloqueja el següent nivell després d'una victòria.
    private void UnlockNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int levelReached = PlayerPrefs.GetInt("levelReached", (int)GameScene.INFORMATION_LEVEL1);

        int nextLevel = currentScene + 1;

        if (nextLevel > levelReached)
        {
            PlayerPrefs.SetInt("levelReached", nextLevel);
            PlayerPrefs.Save();
        }
    }

    // Reinicia el nivell actual.
    public void LevelCurrentRetry()
    {
        AudioManager.audioInstance?.PlayBackgroundMusic();
        SceneLoad(SceneManager.GetActiveScene().buildIndex);
    }

    // Carrega una escena per índex.
    public void SceneLoad(int sceneIndex)
    {
        Time.timeScale = 1f;
        gameActived = false;
        gamePaused = false;
        SceneManager.LoadScene(sceneIndex);
    }

    // Pausa el joc i mostra el panell de pausa.
    public void GamePaused()
    {
        if (!gameActived || gamePaused || levelCurrentController == null) return;

        gamePaused = true;
        Time.timeScale = 0f;
        levelCurrentController.ShowPausePanel();
    }

    // Repren el joc i amaga el panell de pausa.
    public void GameResume()
    {
        if (!gameActived || !gamePaused || levelCurrentController == null) return;

        gamePaused = false;
        Time.timeScale = 1f;
        levelCurrentController.HidePausePanel();
    }

    // Tanca el joc, detectant si s'està en Editor o build.
    public void GameExit()
    {
        Application.Quit();
    }

    // Mostra i desbloqueja el cursor.
    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Amaga i bloqueja el cursor.
    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}