using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("References")]
    [SerializeField] private HUDController hud;
    [SerializeField] private PlayerLife player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Rules")]
    [SerializeField] private int coinsToWin = 5;

    public GameState State { get; private set; } = GameState.Playing;
    public int Coins { get; private set; } = 0;
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera goalCamera;
    [SerializeField] private float introLookTime = 2.5f;

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
        State = GameState.Playing;
        AudioManager.I?.PlayBGM();

        if (playerMovement == null && player != null)
            playerMovement = player.GetComponent<PlayerMovement>();

        hud?.SetCoins(Coins, coinsToWin);
        hud?.ClearMessage();
        hud?.HideRestartButton();

        StartCoroutine(CoStartGame());
    }

    private IEnumerator CoStartGame()
    {
        if (player != null)
            player.SetControlEnabled(false);

        // เริ่มที่กล้องเส้นชัย
        if (goalCamera != null) goalCamera.Priority = 20;
        if (playerCamera != null) playerCamera.Priority = 10;

        yield return new WaitForSeconds(introLookTime);

        // กลับมาที่กล้องผู้เล่น
        if (goalCamera != null) goalCamera.Priority = 9;
        if (playerCamera != null) playerCamera.Priority = 10;

        yield return new WaitForSeconds(0.5f);

        hud?.ShowMessage("3", false);
        AudioManager.I?.PlayCountdown();
        yield return new WaitForSeconds(1f);

        hud?.ShowMessage("2", false);
        AudioManager.I?.PlayCountdown();
        yield return new WaitForSeconds(1f);

        hud?.ShowMessage("1", false);
        AudioManager.I?.PlayCountdown();
        yield return new WaitForSeconds(1f);

        hud?.ShowMessage("GO!", true);
        AudioManager.I?.PlayGo();

        if (player != null)
            player.SetControlEnabled(true);

        if (playerMovement != null)
            playerMovement.StartRun();

        if (enemySpawner != null)
            enemySpawner.StartSpawning();
    }

    public bool IsPlaying => State == GameState.Playing;

    public void AddCoin(int amount)
    {
        if (!IsPlaying) return;

        Coins += amount;
        hud?.SetCoins(Coins, coinsToWin);

        if (Coins >= coinsToWin)
            Win();
    }

    public void Win()
    {
        if (State == GameState.Win) return;

        State = GameState.Win;
        hud?.ShowWin();
        AudioManager.I?.StopRunLoop();
        AudioManager.I?.FadeOutBGM();
        AudioManager.I?.PlayWin();

        if (player != null)
            player.SetControlEnabled(false);

        if (playerMovement != null)
            playerMovement.PlayWinIdleOnBottomLane();
    }

    public void PlayerDied()
    {
        if (!IsPlaying) return;

        State = GameState.GameOver;
        hud?.ShowGameOver();
        AudioManager.I?.StopRunLoop();
        AudioManager.I?.FadeOutBGM();
        AudioManager.I?.PlayGameOver();

        if (player != null)
        {
            player.SetControlEnabled(false);
            player.StopMotion();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetPlayerControl(bool enable)
    {
        if (player != null)
        {
            player.SetControlEnabled(enable);

            if (!enable)
                player.StopMotion();
        }
    }

    public void ShowMessage(string msg, bool autoHide)
    {
        hud?.ShowMessage(msg, autoHide);
    }
}