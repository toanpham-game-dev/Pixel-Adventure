using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUIController : MonoBehaviour
{
    [SerializeField] private GameObject nextLevelButton;
    private void Start()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex >= SceneManager.sceneCountInBuildSettings - 1)
        {
            nextLevelButton.SetActive(false);
        }
    }

    public void OnClickNextLevelButton()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        int levelNumber = int.Parse(currentScene.Replace("Lv", ""));
        levelNumber++;
        SceneLoader.Instance.LoadLevel("Lv" + levelNumber);
    }

    public void OnClickMainMenuButton()
    {
        SceneLoader.Instance.LoadMainMenuScene();
    }

    public void PlaySFX(AudioClip clip)
    {
        SoundManager.Instance.PlaySFX(clip);
    }
}
