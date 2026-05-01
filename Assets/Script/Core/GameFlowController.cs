// using System.Collections;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class GameFlowController : MonoBehaviour
// {
//     public static GameFlowController I { get; private set; }

//     [Header("References")]
//     [SerializeField] private HUDController hud;
//     [SerializeField] private CountdownController countdown;
//     [SerializeField] private PlayerRoot player;
//     [SerializeField] private EnemySpawner enemySpawner;

//     [Header("Game Rules")]
//     [SerializeField] private CameraIntroSequence cameraIntro;
//     [SerializeField] private ScoreManager scoreManager;

//     public GameState State { get; private set; } = GameState.Boot;
//     public int Coins { get; private set; }

//     private void Awake()
//     {
//         if (I != null && I != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         I = this;
//     }

//     private void Start()
//     {
//         Time.timeScale = 1f;

//         Coins = 0;
//         hud?.HideRestartButton();
//         hud?.ClearMessage();

//         AudioManager.I?.PlayBGM();
//         scoreManager?.ResetAll();

//         BeginCountdown();
//     }

//     public void BeginCountdown()
//     {
//         State = GameState.Countdown;

//         player?.SetControlEnabled(false);

//         StartCoroutine(CoStartSequence());
//     }

//     private IEnumerator CoStartSequence()
//     {
//         // 🎬 เล่น intro กล้องก่อน
//         if (cameraIntro != null)
//             yield return StartCoroutine(cameraIntro.PlayIntro());

//         // 🔊 เสียง countdown
//         AudioManager.I?.PlayCountdown();

//         if (countdown != null)
//             countdown.Begin(OnCountdownFinished);
//         else
//             OnCountdownFinished();
//     }

//     private void OnCountdownFinished()
//     {
//         State = GameState.Playing;

//         AudioManager.I?.PlayGo();

//         player?.StartGameplay();
//         enemySpawner?.StartSpawning();
//     }

//     public void GameOver()
//     {
//         if (State == GameState.GameOver) return;

//         State = GameState.GameOver;
//         scoreManager?.FinalizeScore();

//         AudioManager.I?.StopRunLoop();
//         AudioManager.I?.PlayGameOver();
//         AudioManager.I?.FadeOutBGM(0.5f);

//         player?.OnDeath();
//         hud?.ShowGameOver();

//         var enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
//         foreach (var e in enemies)
//             e.SetActiveAI(false);
//     }

//     public void PauseGame()
//     {
//         if (State != GameState.Playing) return;

//         State = GameState.Paused;
//         Time.timeScale = 0f;
//         player?.SetControlEnabled(false);
//     }

//     public void ResumeGame()
//     {
//         if (State != GameState.Paused) return;

//         State = GameState.Playing;
//         Time.timeScale = 1f;
//         player?.SetControlEnabled(true);
//     }

//     public void RestartGame()
//     {
//         Time.timeScale = 1f;
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }

//     public void ShowMessage(string msg, bool autoHide = true)
//     {
//         hud?.ShowMessage(msg, autoHide);
//     }

//     public void AddScore(int amount)
// {
//     if (State != GameState.Playing) return;

//     scoreManager?.AddScore(amount);
// }
// }