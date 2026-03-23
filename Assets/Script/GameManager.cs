using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("References")]
    [SerializeField] private HUDController hud;
    [SerializeField] private PlayerLife player;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Rules")]
    [SerializeField] private int coinsToWin = 5;

    [Header("Respawn")]
    [SerializeField] private float respawnDelay = 0.6f;
    [SerializeField] private float postRespawnInvincible = 1.0f;

    public GameState State { get; private set; } = GameState.Playing;
    public int Coins { get; private set; } = 0;
    public Vector3 RespawnPoint { get; private set; }

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

        if (player != null)
            RespawnPoint = player.transform.position;

        if (playerMovement == null && player != null)
            playerMovement = player.GetComponent<PlayerMovement>();

        hud?.SetCoins(Coins, coinsToWin);
        hud?.ClearMessage();

        StartCoroutine(CoStartGame());
    }

    private IEnumerator CoStartGame()
    {
        if (player != null)
            player.SetControlEnabled(false);

        hud?.ShowMessage("3", false);
        yield return new WaitForSeconds(1f);

        hud?.ShowMessage("2", false);
        yield return new WaitForSeconds(1f);

        hud?.ShowMessage("1", false);
        yield return new WaitForSeconds(1f);

        hud?.ShowMessage("GO!", true);

        if (player != null)
            player.SetControlEnabled(true);

        if (playerMovement != null)
            playerMovement.StartRun();
    }

    public bool IsPlaying => State == GameState.Playing;

    public void RegisterCheckpoint(Vector3 p)
    {
        RespawnPoint = p;
        Debug.Log($"Respawn point: {p}");
    }

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

        if (player != null)
            player.SetControlEnabled(false);
    }

    public void PlayerDied()
    {
        if (!IsPlaying) return;

        State = GameState.Dead;
        hud?.ShowDead();

        if (player != null)
        {
            player.SetControlEnabled(false);
            player.StopMotion();
        }

        StartCoroutine(CoRespawn());
    }

    private IEnumerator CoRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (player != null)
        {
            player.TeleportTo(RespawnPoint);
            player.StopMotion();
            player.ResetHP();
            player.SetControlEnabled(true);
            player.SetInvincible(postRespawnInvincible);
        }

        if (playerMovement != null)
            playerMovement.StartRun();

        if (State != GameState.Win)
            State = GameState.Playing;
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
}