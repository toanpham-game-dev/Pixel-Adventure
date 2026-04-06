using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

    public void ReloadScene()
    {
        string scene = SceneManager.GetActiveScene().name;
        LoadLevel(scene);
    }

    public void LoadLevel(string levelScene)
    {
        StartCoroutine(LoadLevelRoutine(levelScene));
    }

    IEnumerator LoadLevelRoutine(string levelScene)
    {
        // Load level
        yield return SceneManager.LoadSceneAsync(levelScene, LoadSceneMode.Single);

        // Load HUD
        yield return SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive);
    }

    public void LoadAdditionalScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void CloseAdditionalScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}