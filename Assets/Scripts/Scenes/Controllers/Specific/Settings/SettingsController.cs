using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Controlador de la interfície de configuració del joc.
// Aquest script connecta els sliders i toggles de la UI amb l'AudioManager,
// i permet tornar al menú principal.
public class SettingsController : MonoBehaviour
{
    [Header("Referències UI")]
    // Slider per controlar el volum del joc.
    [SerializeField] private Slider sliderVolume;
    // Slider per controlar la brillantor de la pantalla.
    [SerializeField] private Slider sliderBrightness;
    // Toggle per silenciar o activar el so.
    [SerializeField] private Toggle toggleMute;
    // Botó per tornar al menú principal.
    [SerializeField] private Button buttonReturn;

    private void Start()
    {
        // Verificar que les instàncies necessàries estiguin disponibles.
        if (AudioManager.audioInstance == null || GameManager.gameInstance == null || BrightnessManager.brightnessInstance == null) return;

        SlidersConfiguration();
        ToggleMuteConfiguration();
        ReturnMainMenu();
    }

    private void SlidersConfiguration()
    {
        // Slider de volum.
        if (sliderVolume != null)
        {
            sliderVolume.minValue = 0f;
            sliderVolume.maxValue = 1f;
            sliderVolume.value = AudioManager.audioInstance.GetVolume();

            // Eliminem listeners previs per evitar duplicats.
            sliderVolume.onValueChanged.RemoveAllListeners();
            sliderVolume.onValueChanged.AddListener(AudioManager.audioInstance.SetVolume);
        }

        // Slider de brillantor.
        if (sliderBrightness != null)
        {
            sliderBrightness.minValue = 0.3f;
            sliderBrightness.maxValue = 1.0f;
            sliderBrightness.value = BrightnessManager.brightnessInstance.GetBrightness();
            sliderBrightness.onValueChanged.RemoveAllListeners();
            sliderBrightness.onValueChanged.AddListener(BrightnessManager.brightnessInstance.SetBrightness);
        }
    }

    private void ToggleMuteConfiguration()
    {
        if (toggleMute != null)
        {
            // Inicialitza l'estat (toggle ON si no està mutet).
            toggleMute.isOn = !AudioManager.audioInstance.IsMute();
            toggleMute.onValueChanged.RemoveAllListeners();

            toggleMute.onValueChanged.AddListener(OnMuteToggleChanged);
        }
    }

    private void OnMuteToggleChanged(bool value)
    {
        // 'value' és true si el Toggle està marcat.
        // Invertim el valor: si està marcat (value=true), volem desmutear (SetMute(false)).
        AudioManager.audioInstance.SetMute(!value);
    }

    private void ReturnMainMenu()
    {
        if (buttonReturn != null)
        {
            buttonReturn.onClick.RemoveAllListeners();

            buttonReturn.onClick.AddListener(OnReturnButtonClicked);
        }
    }

    // Torna al menú principal.
    private void OnReturnButtonClicked()
    {
        GameManager.gameInstance.SceneLoad((int)GameManager.GameScene.MAIN_MENU);
    }
}