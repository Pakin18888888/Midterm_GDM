using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagers : MonoBehaviour
{
    public static GameManagers Instance;

    public float gameTime = 0f;
    public bool isRunning = true;

    public int stage = 1;
    public float difficulty = 1f;

    public GameObject gameOverUI;
    public bool isGameOver = false;
    public GameObject leaderboardPanel;

    public TMP_Text runText;
    public TMP_Text bestText;
    public ScoreTween runScoreTween;
    public ScoreTween bestScoreTween;

    public PolarityType topLanePolarity;
    public PolarityType bottomLanePolarity;
    private Vector3 leaderboardOriginalScale;

    public bool isRestStage;
    public bool usePolarity = true;
    [SerializeField] private InterstitialAdController interstitialAdController;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gameOverUI.SetActive(false);

        UpdateStageMode();

        Debug.Log("HighScore: " + ScoreboardManager.Instance.GetHighScore());

        leaderboardOriginalScale = leaderboardPanel.transform.localScale;

    }

    void Update()
    {
        if (!isRunning) return;

        gameTime += Time.deltaTime;

        int oldStage = stage;

        if (gameTime < 10f) stage = 1;
        else if (gameTime < 25f) stage = 2;
        else if (gameTime < 45f) stage = 3;
        else if (gameTime < 70f) stage = 4;
        else if (gameTime < 100f) stage = 5;
        else if (gameTime < 140f) stage = 6;
        else if (gameTime < 180f) stage = 7;
        else if (gameTime < 230f) stage = 8;
        else if (gameTime < 280f) stage = 9;
        else
        {
            stage = 10 + Mathf.FloorToInt((gameTime - 280f) / 60f);
        }

        if (oldStage != stage)
        {
            UpdateStageMode();

            StageUI.Instance.ShowStage(stage, isRestStage);

            PlayerPolaritys.Instance.SendMessage(
                "ApplyVisual"
            );
        }

        difficulty = 1f + gameTime * 0.05f;
    }

    public void GameState(bool state)
    {
        isRunning = state;
    }

    public bool CanSpawnCoin()
    {
        return gameTime >= 11f;
    }

    public bool CanSpawnEnemy()
    {
        return stage >= 2;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        isRunning = false;

        int finalScore = ScoreManager.Instance.score;
        int bestScore = ScoreboardManager.Instance.GetHighScore();
        int finalStreak = ScoreManager.Instance.streak;

        // ✅ เปิด UI
        gameOverUI.SetActive(true);
        gameOverUI.transform.localScale = Vector3.one;

        PlayerPrefs.SetInt("PLAYED_ONCE", 1);
        PlayerPrefs.Save();

        // ✅ pause
        Time.timeScale = 0f;

        // ✅ เล่น animation
        runScoreTween.Play(finalScore);
        bestScoreTween.Play(bestScore);

        // ✅ save offline
        ScoreboardManager.Instance.Save(finalScore, finalStreak);

        // ❌ อย่า submit ตรงนี้
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public async void OpenLeaderboard()
    {
        Time.timeScale = 1f;

        gameOverUI.SetActive(false);
        leaderboardPanel.SetActive(true);

        await UGSInitializer.InitTask;

        await SyncOfflineScore();

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
        gameOverUI.SetActive(true);
    }
    bool hasSynced = false;

    public async Task SyncOfflineScore()
    {
        if (hasSynced) return;

        hasSynced = true;

        int bestScore = ScoreboardManager.Instance.GetHighScore();

        await LeaderboardManager.Instance.SubmitScoreSafe(bestScore);

        Debug.Log("Sync Complete");
    }
    
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        if (interstitialAdController != null)
        {
            interstitialAdController.ShowInterstitial(() =>
            {
                SceneManager.LoadScene("MainMenuScene");
            });
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    void UpdateStageMode()
    {
        // ทุกด่านที่หาร 6 ลงตัว = ด่านพัก
        // เช่น 6,12,18

        isRestStage = stage % 6 == 0;

        // ด่านพักปิด polarity
        usePolarity = !isRestStage;

        Debug.Log(
            "Stage: " + stage +
            " Rest: " + isRestStage +
            " UsePolarity: " + usePolarity
        );
    }
}