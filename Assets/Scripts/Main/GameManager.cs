using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void GameOver()
    {
        SceneLoader.Instance.LoadAdditionalScene("Over");
    }

    public void CompleteLevel(int levelIndex, int starEarned)
    {
        int oldStar = PlayerPrefs.GetInt("LevelStar_" + levelIndex, 0);
        int finalStar = Mathf.Max(oldStar, starEarned);

        PlayerPrefs.SetInt("LevelStar_" + levelIndex, finalStar);

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (levelIndex >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", levelIndex + 1);
        }

        PlayerPrefs.Save();

        Debug.Log($"Level {levelIndex} = {finalStar} star");

        SaveToCloud();
    }

#if UNITY_ANDROID
    private async void SaveToCloud()
    {
        if (FirebaseManager.Instance == null) return;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }

        CloudSaveData data = FirebaseManager.Instance.GetLocalData();

        await FirebaseManager.Instance.SaveCloudData(data);
    }
#else
    private void SaveToCloud()
    {
        // Do nothing on PC
    }
#endif

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Reset Progress");
    }
}