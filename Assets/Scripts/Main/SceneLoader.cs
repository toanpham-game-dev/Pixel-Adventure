using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private AnimationController _anim;

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

        _anim = GetComponent<AnimationController>();
    }

    public void LoadMainMenuScene()
    {
        StartCoroutine(LoadSceneWithTransition("MainMenu"));
    }

    public void ReloadScene()
    {
        string scene = SceneManager.GetActiveScene().name;
        LoadLevel(scene);
    }

    public void LoadLevel(string levelScene)
    {
        StartCoroutine(LoadSceneWithTransition(levelScene, loadHUD: true));
    }

    private IEnumerator LoadSceneWithTransition(string sceneName, bool loadHUD = false)
    {
        _anim.PlayAnimation("WipeOut");
        yield return new WaitForSeconds(0.5f);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            yield return null;
        }

        if (loadHUD)
        {
            yield return SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive);
        }

        _anim.PlayAnimation("WipeIn");
    }


    public void LoadAdditionalScene(string sceneName)
    {
        StartCoroutine(LoadAdditionalRoutine(sceneName));
    }

    private IEnumerator LoadAdditionalRoutine(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void CloseAdditionalScene(string sceneName)
    {
        StartCoroutine(CloseAdditionalRoutine(sceneName));
    }

    private IEnumerator CloseAdditionalRoutine(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);
    }
}