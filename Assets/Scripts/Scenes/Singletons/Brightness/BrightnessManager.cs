using UnityEngine;
using UnityEngine.UI;

// Controla la brillantor del joc mitjançant un overlay negre a la pantalla.
public class BrightnessManager : MonoBehaviour
{
    // Instància única del BrightnessManager per accedir-hi des de qualsevol lloc.
    public static BrightnessManager brightnessInstance;

    // Imatge que s'utilitza com a overlay per controlar la brillantor.
    private Image overlayBrightness;
    // Valor de brillantor actual (1 = màxim, 0.3 = mínim).
    private float brightness = 1f;

    // Valor mínim de brillantor permès.
    private const float BRIGHTNESS_MIN = 0.3f;
    // Valor màxim de brillantor permès.
    private const float BRIGHTNESS_MAX = 1f;
    // Ordre de renderització del canvas de brillantor.
    private const int ORDER_BRIGHTNESS = 9999;

    private void Awake()
    {
        // Implementació del patró Singleton.
        if (brightnessInstance != null && brightnessInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        brightnessInstance = this;
        DontDestroyOnLoad(gameObject);

        // Crea l'overlay, carrega la configuració i aplica la brillantor.
        CreateOverlay();
        LoadSettings();
        ApplyBrightness();
    }

    // Crea l'overlay negre que simula la brillantor.
    private void CreateOverlay()
    {
        // Crea el GameObject del canvas.
        GameObject canvasObj = new GameObject("BrightnessCanvas");
        canvasObj.transform.SetParent(transform);
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = ORDER_BRIGHTNESS;

        // Crea el GameObject de l'overlay.
        GameObject overlayObj = new GameObject("BrightnessOverlay");
        overlayObj.transform.SetParent(canvasObj.transform);
        overlayBrightness = overlayObj.AddComponent<Image>();
        overlayBrightness.color = Color.black;
        overlayBrightness.raycastTarget = false;

        // Configura el RectTransform per cobrir tota la pantalla.
        RectTransform rectTransform = overlayObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    // Canvia la brillantor.
    public void SetBrightness(float value)
    {
        // Assegura que el valor estigui dins dels límits permessos.
        brightness = Mathf.Clamp(value, BRIGHTNESS_MIN, BRIGHTNESS_MAX);
        ApplyBrightness();
        SaveSettings();
    }

    // Retorna el valor de brillantor actual.
    public float GetBrightness() { return brightness; }

    // Aplica la brillantor modificant l'opacitat de l'overlay.
    private void ApplyBrightness()
    {
        if (overlayBrightness != null)
        {
            // Calcula l'alfa basat en la brillantor (més brillantor = menys opacitat).
            float alpha = 1f - brightness;
            overlayBrightness.color = new Color(0f, 0f, 0f, alpha);
        }
    }

    // Guarda la configuració de brillantor a PlayerPrefs.
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.Save();
    }

    // Carrega la configuració de brillantor des de PlayerPrefs.
    private void LoadSettings()
    {
        brightness = PlayerPrefs.GetFloat("Brightness", 1f);
    }
}