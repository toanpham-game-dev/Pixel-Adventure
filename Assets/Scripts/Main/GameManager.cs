using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int _levelIndex;
    [SerializeField] private int _starEarned;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CompleteLevel(_levelIndex, _starEarned);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ResetProgress();
        }
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

        Debug.Log($"Level {levelIndex} completed with {finalStar} stars");
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Progress Reset");
    }
}
