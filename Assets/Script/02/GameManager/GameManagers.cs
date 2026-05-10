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

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gameOverUI.SetActive(false);

        UpdateLanePolarity();

        Debug.Log("HighScore: " + ScoreboardManager.Instance.GetHighScore());

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
        else stage = 10;

        if (oldStage != stage)
        {
            UpdateLanePolarity();
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
        return gameTime >= 15f;
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
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
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

    void UpdateLanePolarity()
    {
        bool flip = stage % 2 == 0;

        if (flip)
        {
            topLanePolarity = PolarityType.Positive;
            bottomLanePolarity = PolarityType.Negative;
        }
        else
        {
            topLanePolarity = PolarityType.Negative;
            bottomLanePolarity = PolarityType.Positive;
        }

        Debug.Log(
            "Top: " + topLanePolarity +
            " Bottom: " + bottomLanePolarity
        );
    }
}