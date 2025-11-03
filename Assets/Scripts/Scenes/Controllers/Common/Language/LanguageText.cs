using UnityEngine;
using TMPro;

public class LanguageText : MonoBehaviour
{
    // Referència al component TMP_Text on es mostrarà el text.
    public TMP_Text textUI;

    // Llista de textos en diferents idiomes (ex: 0 = Espanyol, 1 = Anglès, 2 = Català...).
    public string[] textsForLanguages = new string[3];

    void Awake()
    {
        // Obtenir automàticament el component TMP_Text si no està assignat.
        if (textUI == null)
            textUI = GetComponent<TMP_Text>();
    }

    void Start()
    {
        // Verificar que el LanguageManager estigui disponible.
        if (LanguageManager.instanceLanguage != null)
        {
            // Registrar aquest text en el gestor d'idiomes.
            LanguageManager.instanceLanguage.RegisterText(this);

            // Subscriure's a l'esdeveniment de canvi d'idioma.
            LanguageManager.instanceLanguage.toChangeLanguage += ToChangeLanguage;

            // Mostrar el text en l'idioma actual.
            TextUpdate(LanguageManager.instanceLanguage.languageCurrent);
        }
    }

    void OnDestroy()
    {
        // Netejar les referències quan es destrueix l'objecte.
        if (LanguageManager.instanceLanguage != null)
        {
            // Eliminar aquest text de la llista del gestor.
            LanguageManager.instanceLanguage.RemoveText(this);

            // Desubscriure's de l'esdeveniment.
            LanguageManager.instanceLanguage.toChangeLanguage -= ToChangeLanguage;
        }
    }

    // Mètode que es crida quan canvia l'idioma.
    void ToChangeLanguage(int newLanguage)
    {
        TextUpdate(newLanguage);
    }

    // Actualitza el text segons l'idioma seleccionat.
    public void TextUpdate(int indexLanguage)
    {
        if (textUI == null) return;

        // Si existeix un text en aquest idioma, l'utilitza.
        if (indexLanguage < textsForLanguages.Length && !string.IsNullOrEmpty(textsForLanguages[indexLanguage]))
        {
            textUI.text = textsForLanguages[indexLanguage];
        }
        // Si no n'hi ha, utilitzar el primer idioma com a fallback.
        else if (textsForLanguages.Length > 0 && !string.IsNullOrEmpty(textsForLanguages[0]))
        {
            textUI.text = textsForLanguages[0];
        }
    }
}