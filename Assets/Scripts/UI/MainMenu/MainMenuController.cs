using DG.Tweening;
using UnityEngine;
using System.Collections;
public class MainMenuController : MonoBehaviour
{
    [Header("Login UI")]
    public GameObject loginPanel;
    public GameObject loadingPanel;

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
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);

        StartCoroutine(CheckLoginWhenReady());
    }

    private void OnLoginSuccess()
    {
        loadingPanel.SetActive(false);
        loginPanel.SetActive(false);

        OpenMainMenu();
    }

    public void OnGuestLoginClick()
    {
        loadingPanel.SetActive(true);

        FirebaseManager.Instance.OnDataLoaded -= OnLoginSuccess;
        FirebaseManager.Instance.OnDataLoaded += OnLoginSuccess;

        FirebaseManager.Instance.OnGuestLogin();
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
    }

    // ================= MAIN MENU =================

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
                .SetDelay(delay);

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
                .SetDelay(delay);

            delay += 0.05f;
        }

        DOVirtual.DelayedCall(0.35f, () =>
        {
            mainMenuPanel.SetActive(false);
        });
    }

    // ================= SETTINGS =================

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        settingsBackground.alpha = 0;
        settingsBackground.DOFade(1f, 0.25f);

        foreach (RectTransform panel in settingsChildPanels)
        {
            panel.localScale = Vector3.zero;

            panel.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
        }
    }

    public void CloseSettings()
    {
        settingsBackground.DOFade(0, 0.2f);

        foreach (RectTransform panel in settingsChildPanels)
        {
            panel.DOScale(0f, 0.25f).SetEase(Ease.InBack);
        }

        DOVirtual.DelayedCall(0.25f, () =>
        {
            settingsPanel.SetActive(false);
        });
    }

    // ================= LEVEL =================

    public void OpenLevelSelection()
    {
        levelSelectPanel.SetActive(true);

        levelBackground.alpha = 0;
        levelBackground.DOFade(1f, 0.25f);

        foreach (RectTransform panel in levelChildPanels)
        {
            panel.localScale = Vector3.zero;

            panel.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
        }
    }

    public void CloseLevelSelection()
    {
        levelBackground.DOFade(0, 0.2f);

        foreach (RectTransform panel in levelChildPanels)
        {
            panel.DOScale(0f, 0.25f).SetEase(Ease.InBack);
        }

        DOVirtual.DelayedCall(0.25f, () =>
        {
            levelSelectPanel.SetActive(false);
        });
    }

    private IEnumerator CheckLoginWhenReady()
    {
        yield return new WaitUntil(() =>
            FirebaseManager.Instance != null &&
            FirebaseManager.Instance.IsInitialized
        );

        if (FirebaseManager.Instance.IsLoggedIn())
        {
            loginPanel.SetActive(false);
            loadingPanel.SetActive(true);

            FirebaseManager.Instance.OnDataLoaded -= OnLoginSuccess;
            FirebaseManager.Instance.OnDataLoaded += OnLoginSuccess;

            FirebaseManager.Instance.AutoLogin();
        }
        else
        {
            loginPanel.SetActive(true);
            loadingPanel.SetActive(false);
        }
    }
}