using UnityEngine;
using System.Collections.Generic;

// Singleton que gestiona els idiomes del joc.
// Permet canviar l'idioma en temps real i notificar a tots els textos multillenguatge.
public class LanguageManager : MonoBehaviour
{
    // SINGLETON
    // Instància global per accedir des de qualsevol script.
    public static LanguageManager instanceLanguage;

    // CONFIGURACIÓN DE IDIOMA
    [Header("Configuration of language")]
    // Índex de l'idioma actual (0, 1, 2...).
    public int languageCurrent = 0;
    // Quantitat total d'idiomes disponibles.
    public int quantityLanguages = 3;

    // Llista de tots els textos multillenguatge registrats en l'escena.
    private List<LanguageText> listTexts = new List<LanguageText>();


    // Esdeveniment que es dispara cada vegada que es canvia l'idioma.
    // Els scripts que vulguin reaccionar poden subscriure's.
    public System.Action<int> toChangeLanguage;

    private void Awake()
    {
        // Assegurem que només existeixi un LanguageManager.
        if (instanceLanguage == null)
        {
            instanceLanguage = this;
            // Persisteix entre escenes.
            DontDestroyOnLoad(gameObject);

            // Carreguem l'idioma guardat a la memòria (0 si no n'hi ha).
            languageCurrent = PlayerPrefs.GetInt("Language", 0);
        }
        else
        {
            // Si ja existeix, destruïm el duplicat.
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Busquem tots els textos multillenguatge en l'escena i els registrem.
        SearchAllTexts();

        // Actualitzem els textos a l'idioma actual.
        UpdateAllTexts();
    }

    // MÈTODES PRINCIPALS

    // Cerca tots els objectes LanguageText actius en l'escena i els guarda a la llista.
    public void SearchAllTexts()
    {
        listTexts.Clear();

        // Cerca tots els components LanguageText actius.
        LanguageText[] foundTexts = FindObjectsByType<LanguageText>(FindObjectsSortMode.InstanceID);
        listTexts.AddRange(foundTexts);
    }

    // Canvia al següent idioma de forma cíclica.
    public void ToChangeNextLanguage()
    {
        languageCurrent = (languageCurrent + 1) % quantityLanguages;

        // Guardem l'idioma seleccionat per a la propera partida.
        PlayerPrefs.SetInt("Language", languageCurrent);

        // Actualitzem tots els textos registrats.
        UpdateAllTexts();

        // Disparem l'esdeveniment per a scripts subscrits.
        toChangeLanguage?.Invoke(languageCurrent);
    }

    // Actualitza tots els textos registrats amb l'idioma actual.
    private void UpdateAllTexts()
    {
        foreach (LanguageText texto in listTexts)
        {
            if (texto != null)
            {
                texto.TextUpdate(languageCurrent);
            }
        }
    }

    // Registra un text manualment a la llista de textos a actualitzar.
    public void RegisterText(LanguageText text)
    {
        if (!listTexts.Contains(text))
            listTexts.Add(text);
    }


    // Treu un text de la llista quan ja no està en ús.
    public void RemoveText(LanguageText text)
    {
        listTexts.Remove(text);
    }

}
