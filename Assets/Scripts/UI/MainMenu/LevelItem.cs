using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public int levelIndex;
    public Button levelButton;

    public GameObject[] blackStars;
    public GameObject[] goldStars;

    public void SetupLevel(int unlockedLevel, int starCount)
    {
        bool isUnlocked = levelIndex <= unlockedLevel;
        gameObject.SetActive(isUnlocked);

        // Turn off all black star
        foreach (GameObject star in blackStars)
            star.SetActive(false);

        // Turn off all gold star
        foreach (GameObject star in goldStars)
            star.SetActive(false);

        // Return if level never play before
        if (starCount == 0)
            return;

        // Turn on black stars
        foreach (GameObject star in blackStars)
            star.SetActive(true);

        // Turn on gold stars
        for (int i = 0; i < starCount; i++)
        {
            goldStars[i].SetActive(true);
        }
    }
}