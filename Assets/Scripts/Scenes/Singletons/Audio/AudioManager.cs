using UnityEngine;

// Singleton que controla l'àudio del joc: música i efectes.
public class AudioManager : MonoBehaviour
{
    // Instància única del AudioManager per accedir-hi des de qualsevol lloc.
    public static AudioManager audioInstance;

    [Header("Audio Sources")]
    // Font d'àudio principal per a la música.
    public AudioSource music;
    // Font d'àudio per als efectes de so.
    public AudioSource effects;

    [Header("Clips")]
    // Clip de so per a la recol·lecció d'objectes.
    public AudioClip soundRecollectibleCollect;
    // Clip de so per a l'obertura i tancament de portes.
    public AudioClip soundDoor;
    // Clip de so per a l'encesa i apagada de llums.
    public AudioClip soundLightSwitch;
    // Clip de so per a la derrota.
    public AudioClip soundDefeat;
    // Clip de so per a la victòria.
    public AudioClip soundVictory;
    // Clip de música de fons.
    public AudioClip musicBackground;

    // Volum actual de l'àudio.
    private float volume = 1f;
    // Indica si l'àudio està silenciat.
    private bool isMute = false;

    private void Awake()
    {
        // Implementació del patró Singleton.
        if (audioInstance != null && audioInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        audioInstance = this;
        DontDestroyOnLoad(gameObject);

        // Inicialització de les fonts d'àudio si no estan assignades.
        if (music == null) music = GetComponent<AudioSource>();
        if (effects == null)
        {
            GameObject effectsObj = new GameObject("EffectsAudio");
            effectsObj.transform.SetParent(transform);
            effects = effectsObj.AddComponent<AudioSource>();
        }

        // Carrega la configuració guardada.
        LoadSettings();
    }

    // Reprodueix un efecte de so de recol·lectar objecte.
    public void PlaySoundCollect()
    {
        if (effects != null && soundRecollectibleCollect != null && !isMute)
            effects.PlayOneShot(soundRecollectibleCollect);
    }

    // Reprodueix un efecte de so d'obrir o tancar la porta.
    public void PlayDoorSound()
    {
        if (effects != null && soundDoor != null && !isMute)
            effects.PlayOneShot(soundDoor);
    }

    // Reprodueix un efecte de so d'encendre o apagar la llum.
    public void PlayLightSwitchSound()
    {
        if (effects != null && soundLightSwitch != null && !isMute)
            effects.PlayOneShot(soundLightSwitch);
    }

    // Reprodueix la música de derrota.
    public void PlaySoundDefeat()
    {
        if (soundDefeat != null && music != null && !isMute)
        {
            music.Stop();
            music.clip = soundDefeat;
            music.loop = false;
            music.Play();
        }
    }

    // Reprodueix la música de victòria.
    public void PlaySoundVictory()
    {
        if (soundVictory != null && music != null && !isMute)
        {
            music.Stop();
            music.clip = soundVictory;
            music.loop = false;
            music.Play();
        }
    }

    // Reprodueix la música de fons.
    public void PlayBackgroundMusic()
    {
        if (musicBackground != null && music != null && !isMute)
        {
            // Evitar reiniciar la música si ja s'està reproduint.
            if (music.isPlaying && music.clip == musicBackground) return;
            music.Stop();
            music.clip = musicBackground;
            music.loop = true;
            music.Play();
        }
    }

    // Ajusta el volum de l'àudio.
    public void SetVolume(float v)
    {
        volume = Mathf.Clamp01(v);
        if (music != null) music.volume = volume;
        SaveSettings();
    }

    // Retorna el volum actual.
    public float GetVolume() => volume;

    // Silencia o activa l'àudio.
    public void SetMute(bool mute)
    {
        isMute = mute;
        if (music != null) music.mute = isMute;
        SaveSettings();
    }

    // Retorna si l'àudio està silenciat.
    public bool IsMute() { return isMute; }

    // Guarda la configuració d'àudio a PlayerPrefs.
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("Mute", isMute ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Carrega la configuració d'àudio des de PlayerPrefs.
    private void LoadSettings()
    {
        volume = PlayerPrefs.GetFloat("Volume", 1f);
        isMute = PlayerPrefs.GetInt("Mute", 0) == 1;

        // Aplica la configuració carregada a les fonts d'àudio.
        if (music != null)
        {
            music.volume = volume;
            music.mute = isMute;
        }
    }
}