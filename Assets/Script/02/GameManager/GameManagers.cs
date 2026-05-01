using UnityEngine;

public class GameManagers : MonoBehaviour
{
    public static GameManagers Instance;

    public float gameTime = 0f;

    public float coinStartTime = 5f;
    public float enemyStartDelayAfterCoin = 3f;

    public bool isRunning = true; // 👈 เพิ่ม
    public int stage = 1;

    void Awake()
    {
        Instance = this;
    }

    public float difficulty = 1f;

    void Update()
    {
        if (!isRunning) return;

        gameTime += Time.deltaTime;
        if (gameTime > 20f)
        {
            stage = 2;
        }

        difficulty += Time.deltaTime * 0.05f;
    }

    // 🎯 ใช้ตอนเริ่ม/จบเกม
    public void GameState(bool state)
    {
        isRunning = state;
    }

    public bool CanSpawnCoin()
    {
        return isRunning && gameTime >= coinStartTime;
    }

    public bool CanSpawnEnemy()
    {
        return isRunning && gameTime >= (coinStartTime + enemyStartDelayAfterCoin);
    }
}