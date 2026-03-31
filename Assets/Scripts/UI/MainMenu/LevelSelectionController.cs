using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionController : MonoBehaviour
{
    public LevelItem[] levels;

    private void Start()
    {
        UpdateLevelPanel();
    }

    public void UpdateLevelPanel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levels.Length; i++)
        {
            int star = PlayerPrefs.GetInt("LevelStar_" + (i + 1), 0);

            levels[i].SetupLevel(unlockedLevel, star);
        }
    }

    public void LoadLevel(string levelName)
    {
        SceneLoader.Instance.LoadLevel(levelName);
    }
}