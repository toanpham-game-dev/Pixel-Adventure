using DG.Tweening;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;

    [Header("Main Menu Panel")]
    public CanvasGroup menuBackground;
    public RectTransform[] menuChildPanels;

    [Header("Settings Panel")]
    public CanvasGroup settingsBackground;
    public RectTransform[] settingsChildPanels;

    [Header("Level Selection Panel")]
    public CanvasGroup levelBackground;
    public RectTransform[] levelChildPanels;

    Vector2[] menuStartPositions;

    private void Awake()
    {
        SaveMenuPositions();
    }

    private void Start()
    {
        OpenMainMenu();
    }

    private void SaveMenuPositions()
    {
        menuStartPositions = new Vector2[menuChildPanels.Length];

        for (int i = 0; i < menuChildPanels.Length; i++)
        {
            menuStartPositions[i] = menuChildPanels[i].anchoredPosition;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    // =========================
    // MAIN MENU
    // =========================

    public void OpenMainMenu()
    {
        mainMenuPanel.SetActive(true);

        menuBackground.alpha = 0;
        menuBackground.DOFade(1f, 0.25f);

        float delay = 0;

        for (int i = 0; i < menuChildPanels.Length; i++)
        {
            RectTransform panel = menuChildPanels[i];

            panel.DOKill();

            panel.anchoredPosition = menuStartPositions[i] + new Vector2(-300, 0);

            panel.DOAnchorPos(menuStartPositions[i], 0.4f)
                .SetEase(Ease.OutCubic)
                .SetDelay(delay)
                .SetLink(panel.gameObject);

            delay += 0.08f;
        }
    }

    public void CloseMainMenu()
    {
        menuBackground.DOFade(0, 0.2f);

        float delay = 0;

        for (int i = 0; i < menuChildPanels.Length; i++)
        {
            RectTransform panel = menuChildPanels[i];

            panel.DOKill();

            panel.DOAnchorPos(menuStartPositions[i] + new Vector2(-300, 0), 0.3f)
                .SetEase(Ease.InCubic)
                .SetDelay(delay)
                .SetLink(panel.gameObject);

            delay += 0.05f;
        }

        DOVirtual.DelayedCall(0.35f, () =>
        {
            mainMenuPanel.SetActive(false);
        });
    }

    // =========================
    // SETTINGS
    // =========================

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        settingsBackground.alpha = 0;
        settingsBackground.DOFade(1f, 0.25f);

        foreach (RectTransform panel in settingsChildPanels)
        {
            panel.DOKill();

            panel.localScale = Vector3.zero;

            panel.DOScale(1f, 0.35f)
                .SetEase(Ease.OutBack)
                .SetLink(panel.gameObject);
        }
    }

    public void CloseSettings()
    {
        settingsBackground.DOFade(0, 0.2f);

        foreach (RectTransform panel in settingsChildPanels)
        {
            panel.DOKill();

            panel.DOScale(0f, 0.25f)
                .SetEase(Ease.InBack)
                .SetLink(panel.gameObject);
        }

        DOVirtual.DelayedCall(0.25f, () =>
        {
            settingsPanel.SetActive(false);
        });
    }

    // =========================
    // LEVEL SELECTION
    // =========================

    public void OpenLevelSelection()
    {
        levelSelectPanel.SetActive(true);

        levelBackground.alpha = 0;
        levelBackground.DOFade(1f, 0.25f);

        foreach (RectTransform panel in levelChildPanels)
        {
            panel.DOKill();

            panel.localScale = Vector3.zero;

            panel.DOScale(1f, 0.35f)
                .SetEase(Ease.OutBack)
                .SetLink(panel.gameObject);
        }
    }

    public void CloseLevelSelection()
    {
        levelBackground.DOFade(0, 0.2f);

        foreach (RectTransform panel in levelChildPanels)
        {
            panel.DOKill();

            panel.DOScale(0f, 0.25f)
                .SetEase(Ease.InBack)
                .SetLink(panel.gameObject);
        }

        DOVirtual.DelayedCall(0.25f, () =>
        {
            levelSelectPanel.SetActive(false);
        });
    }
}