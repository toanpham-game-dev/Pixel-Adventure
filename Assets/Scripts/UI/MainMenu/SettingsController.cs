using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Scrollbar _volumeScrollbar;

    void Start()
    {
        _volumeScrollbar.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    public void SetVolume()
    {
        SoundManager.Instance.SetMasterVolume(_volumeScrollbar.value);
    }

    public void ToggleMusic(bool enabled)
    {
        SoundManager.Instance.ToggleMusic(enabled);
    }

    public void ToggleSFX(bool enabled)
    {
        SoundManager.Instance.ToggleSFX(enabled);
    }

    public void PlaySFX(AudioClip audioClip)
    {
        SoundManager.Instance.PlaySFX(audioClip);
    }    
}