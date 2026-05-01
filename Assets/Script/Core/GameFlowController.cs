using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController I { get; private set; }

    [Header("References")]
    [SerializeField] private HUDController hud;
    [SerializeField] private CountdownController countdown;
    [SerializeField] private PlayerRoot player;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Game Rules")]
    [SerializeField] private int coinsToWin = 5;
    [SerializeField] private CameraIntroSequence cameraIntro;

    public GameState State { get; private set; } = GameState.Boot;
    public int Coins { get; private set; }

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        Coins = 0;
        hud?.SetCoins(Coins, coinsToWin);
        hud?.HideRestartButton();
        hud?.ClearMessage();

        AudioManager.I?.PlayBGM();

        BeginCountdown();
    }

    public void BeginCountdown()
    {
        State = GameState.Countdown;

        player?.SetControlEnabled(false);

        StartCoroutine(CoStartSequence());
    }

    private IEnumerator CoStartSequence()
    {
        // 🎬 เล่น intro กล้องก่อน
        if (cameraIntro != null)
            yield return StartCoroutine(cameraIntro.PlayIntro());

        // 🔊 เสียง countdown
        AudioManager.I?.PlayCountdown();

        if (countdown != null)
            countdown.Begin(OnCountdownFinished);
        else
            OnCountdownFinished();
    }

    private void OnCountdownFinished()
    {
        State = GameState.Playing;

        AudioManager.I?.PlayGo();

        player?.StartGameplay();
        enemySpawner?.StartSpawning();
    }

    public void AddCoin(int amount)
    {
        if (State != GameState.Playing) return;

        Coins += amount;
        hud?.SetCoins(Coins, coinsToWin);

        if (Coins >= coinsToWin)
            Win();
    }

    public void Win()
    {
        if (State == GameState.Win) return;

        State = GameState.Win;

        AudioManager.I?.StopRunLoop();
        AudioManager.I?.PlayWin();
        AudioManager.I?.FadeOutBGM(0.5f);

        player?.OnWin();
        hud?.ShowWin();
    }

    public void GameOver()
    {
        if (State == GameState.GameOver) return;

        State = GameState.GameOver;

        AudioManager.I?.StopRunLoop();
        AudioManager.I?.PlayGameOver();
        AudioManager.I?.FadeOutBGM(0.5f);

        player?.OnDeath();
        hud?.ShowGameOver();

        var enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach (var e in enemies)
            e.SetActiveAI(false);
    }

    public void PauseGame()
    {
        if (State != GameState.Playing) return;

        State = GameState.Paused;
        Time.timeScale = 0f;
        player?.SetControlEnabled(false);
    }

    public void ResumeGame()
    {
        if (State != GameState.Paused) return;

        State = GameState.Playing;
        Time.timeScale = 1f;
        player?.SetControlEnabled(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowMessage(string msg, bool autoHide = true)
    {
        hud?.ShowMessage(msg, autoHide);
    }
}