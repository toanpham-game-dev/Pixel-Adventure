using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public float masterVolume = 1f;

    bool musicEnabled = true;
    bool sfxEnabled = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        ApplyVolume();
    }

    void ApplyVolume()
    {
        musicSource.volume = musicEnabled ? masterVolume : 0;
        sfxSource.volume = sfxEnabled ? masterVolume : 0;
    }

    // =====================
    // MASTER VOLUME
    // =====================

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        ApplyVolume();
    }

    // =====================
    // MUSIC
    // =====================

    public void ToggleMusic(bool enabled)
    {
        musicEnabled = enabled;
        PlayerPrefs.SetInt("MusicEnabled", enabled ? 1 : 0);
        ApplyVolume();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    // =====================
    // SFX
    // =====================

    public void ToggleSFX(bool enabled)
    {
        sfxEnabled = enabled;
        PlayerPrefs.SetInt("SFXEnabled", enabled ? 1 : 0);
        ApplyVolume();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!sfxEnabled) return;

        sfxSource.PlayOneShot(clip, masterVolume);
    }
}