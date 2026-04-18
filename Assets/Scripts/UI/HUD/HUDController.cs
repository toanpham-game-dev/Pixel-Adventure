using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public void OnPauseButtonClick()
    {
        SceneLoader.Instance.LoadAdditionalScene("Paused");
    }

    public void PlaySFX(AudioClip clip)
    {
        SoundManager.Instance.PlaySFX(clip);
    }
}
