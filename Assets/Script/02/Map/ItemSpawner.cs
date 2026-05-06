using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform player;

    public float spawnDistance = 15f;
    public float spacing = 1.5f;

    public float bottomY = -3.6f;
    public float topY = -1.8f;
    public float yOffset = 1f;

    private float lastSpawnX = 0f;
    private bool initialized = false;

    void Update()
    {
        if (!GameManagers.Instance.CanSpawnCoin())
            return;

        if (player == null) return;

        if (!initialized)
        {
            lastSpawnX = player.position.x + spawnDistance;
            initialized = true;
        }

        float targetX = player.position.x + spawnDistance;

        while (lastSpawnX < targetX)
        {
            SpawnCoinLine(lastSpawnX);
            lastSpawnX += spacing;
        }
    }

    void SpawnCoinLine(float x)
    {
        int stage = GameManagers.Instance.stage;

        // 🔥 ล่าง 1 เหรียญเสมอ
        SpawnCoin(x, bottomY + yOffset);

        // 🔥 บน 1 เหรียญ (stage 2+)
        if (stage >= 2)
        {
            SpawnCoin(x, topY - yOffset);
        }
    }

    void SpawnCoin(float x, float y)
    {
        Instantiate(coinPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }
}