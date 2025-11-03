using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Permet que un botó canviï l'idioma del joc.
// A més actualitza el text del botó per mostrar l'idioma actual.
public class LanguageButton : MonoBehaviour
{
    // Referència al text del botó (TMP_Text) que mostrarà el nom de l'idioma.
    public TMP_Text textButton;

    // Llista de noms d'idiomes que apareixeràn al botó.
    // L'ordre ha de coincidir amb els índexs de LanguageManager.
    public string[] nameLanguages = { "ESPAÑOL", "CATALÀ", "ENGLISH" };

    void Start()
    {
        // Obtenir component Button del mateix GameObject.
        Button button = GetComponent<Button>();
        if (button != null)
           // Assignar l'esdeveniment WhenPressingButton en fer clic.
            button.onClick.AddListener(WhenPressingButton);

        // Subscriure's a l'esdeveniment de canvi d'idioma del LanguageManager.
        // Això permet actualitzar el text si l'idioma canvia des d'un altre lloc.
        if (LanguageManager.instanceLanguage != null)
            LanguageManager.instanceLanguage.toChangeLanguage += ToChangeLanguage;

        // Mostrar l'idioma correcte a l'iniciar l'escena.
        UpdateTextButton();
    }

    void OnDestroy()
    {
         // Desubscriure's de l'esdeveniment per evitar errors en destruir l'objecte.
        if (LanguageManager.instanceLanguage != null)
            LanguageManager.instanceLanguage.toChangeLanguage -= ToChangeLanguage;
    }

    // Canvia al següent idioma del LanguageManager.
    void WhenPressingButton()
    {
        LanguageManager.instanceLanguage?.ToChangeNextLanguage();
    }


    // Actualitza el text del botó a l'idioma actual.
    void ToChangeLanguage(int newLanguage)
    {
        UpdateTextButton();
    }

    // Actualitza el text del botó per mostrar l'idioma actual.
    void UpdateTextButton()
    {
        // Validacions per seguretat.
        if (textButton == null || LanguageManager.instanceLanguage == null) return;

        int language = LanguageManager.instanceLanguage.languageCurrent;

        // Si existeix un nom per a aquest idioma, el mostrem.
        if (language < nameLanguages.Length)
            textButton.text = nameLanguages[language];
    }
}
