using UnityEngine;
using UnityEngine.InputSystem;

public class PausedController : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;

        PlayerInput playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput != null)
        {
            playerInput.DisableInput();
        }
    }

    public void ResumeGame()
    {
        PlayerInput playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput != null)
        {
            playerInput.EnableInput();
        }

        Time.timeScale = 1f;

        SceneLoader.Instance.CloseAdditionalScene("Paused");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.ReloadScene();
    }

    public void PlaySFX(AudioClip clip)
    {
        SoundManager.Instance.PlaySFX(clip);
    }
}
