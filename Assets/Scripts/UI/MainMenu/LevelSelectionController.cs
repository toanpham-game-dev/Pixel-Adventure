using UnityEngine;

public class LevelSelectionController : MonoBehaviour
{
    public LevelItem[] levels;
    public AudioClip buttonSound;

#if UNITY_ANDROID
    private void OnEnable()
    {
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.OnDataLoaded += UpdateLevelPanel;
        }
    }
#else
    private void OnEnable() { }
#endif

    private void Start()
    {
        UpdateLevelPanel();
    }

    private void OnDisable()
    {
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.OnDataLoaded -= UpdateLevelPanel;
        }
    }

    public void UpdateLevelPanel()
    {
        if (levels == null || levels.Length == 0)
            return;

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] == null)
                continue;

            int star = PlayerPrefs.GetInt("LevelStar_" + (i + 1), 0);

            levels[i].SetupLevel(unlockedLevel, star);
        }

        Debug.Log("UI Level Updated");
    }

    public void LoadLevel(string levelName)
    {
        SceneLoader.Instance.LoadLevel(levelName);
    }

    public void PlaySFX()
    {
        SoundManager.Instance.PlaySFX(buttonSound);
    }
}