using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject profilePanel;
    public GameObject leaderboardPanel;
    public RectTransform logo;
    [SerializeField] private GameObject lockedPopup;
    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform leaderboardButton;
    [SerializeField] private RectTransform profileButton;
    private Dictionary<RectTransform, Vector3> originalScales = new Dictionary<RectTransform, Vector3>();
    private Vector3 leaderboardOriginalScale;
    private Vector3 profileOriginalScale;
    const string PLAYED_KEY = "PLAYED_ONCE";

    void Start()
    {
        LeanTween.moveY(
            logo,
            logo.anchoredPosition.y + 20f,
            1f
        )
        .setLoopPingPong()
        .setEaseInOutSine();

        originalScales[playButton] = playButton.localScale;

        originalScales[leaderboardButton] = leaderboardButton.localScale;

        originalScales[profileButton] = profileButton.localScale;

        leaderboardOriginalScale = leaderboardPanel.transform.localScale;

        profileOriginalScale = profilePanel.transform.localScale;

        AnimateButtonsLoop();
    }

    void AnimateButtonsLoop()
    {
        JellyButton(playButton, 0f);
        JellyButton(leaderboardButton, 0.3f);
        JellyButton(profileButton, 0.6f);
    }

    void JellyButton(RectTransform button, float delay)
    {
        if (button == null) return;

        if (!originalScales.ContainsKey(button))
            return;

        Vector3 baseScale = originalScales[button];

        Vector3 squash =
            new Vector3(
                baseScale.x * 0.9f,
                baseScale.y * 1.1f,
                1f
            );

        Vector3 stretch =
            new Vector3(
                baseScale.x * 1.1f,
                baseScale.y * 0.9f,
                1f
            );

        LeanTween.delayedCall(delay, () =>
        {
            if (button == null) return;

            LeanTween.scale(
                button,
                squash,
                0.15f
            ).setEaseInOutSine()
            .setOnComplete(() =>
            {
                if (button == null) return;

                LeanTween.scale(
                    button,
                    stretch,
                    0.15f
                ).setEaseInOutSine()
                .setOnComplete(() =>
                {
                    if (button == null) return;

                    LeanTween.scale(
                        button,
                        baseScale,
                        0.15f
                    ).setEaseOutBack();
                });
            });
        });

        LeanTween.delayedCall(delay + 3f, () =>
        {
            if (button == null) return;

            JellyButton(button, 0f);
        });
    }

    void OnDestroy()
    {
        LeanTween.cancelAll();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public async void OpenLeaderboard()
    {
        if (PlayerPrefs.GetInt("PLAYED_ONCE", 0) == 0)
        {
            lockedPopup.SetActive(true);

            lockedPopup.transform.localScale = Vector3.zero;

            LeanTween.scale(
                lockedPopup,
                new Vector3(0.2293994f, 0.2293994f, 0.2293994f),
                0.25f
            ).setEaseOutBack();

            LeanTween.delayedCall(1.5f, () =>
            {
                LeanTween.scale(
                    lockedPopup,
                    Vector3.zero,
                    0.2f
                ).setEaseInBack()
                .setOnComplete(() =>
                {
                    lockedPopup.SetActive(false);
                });
            });

            return;
        }

        leaderboardPanel.SetActive(true);

        await UGSInitializer.InitTask;

        FindObjectOfType<LeaderboardUI>().LoadOnline();

        leaderboardPanel.transform.localScale = Vector3.zero;

        LeanTween.scale(
            leaderboardPanel,
            leaderboardOriginalScale,
            0.25f
        ).setEaseOutBack();
    }

    public void CloseLeaderboard()
    {
        LeanTween.scale(
        leaderboardPanel,
        Vector3.zero,
        0.2f
        ).setEaseInBack()
        .setOnComplete(() =>
        {
            leaderboardPanel.SetActive(false);

            leaderboardPanel.transform.localScale =
                leaderboardOriginalScale;
        });
    }

    public void OpenProfile()
    {
        if (profilePanel.activeSelf) return;

        profilePanel.SetActive(true);

        profilePanel.transform.localScale = Vector3.zero;

        LeanTween.scale(
            profilePanel,
            profileOriginalScale,
            0.25f
        ).setEaseOutBack();
    }

    public void CloseProfile()
    {
        LeanTween.scale(
            profilePanel,
            Vector3.zero,
            0.2f
        ).setEaseInBack()
        .setOnComplete(() =>
        {
            profilePanel.SetActive(false);

            profilePanel.transform.localScale =
                profileOriginalScale;
        });
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}