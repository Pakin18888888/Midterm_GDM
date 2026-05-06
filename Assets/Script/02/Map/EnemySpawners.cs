using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spacing = 2.5f;
    public int maxEnemyOnScreen = 5;
    public bool isTopLane = false;
    float groundOffset = 0.8f; // ปรับเอา
    public float bottomOffset = 0.8f;
    public float topOffset = -0.5f;
    bool hasSpawned = false;

    public void Spawn()
    {
        if (hasSpawned) return;

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        if (!GameManagers.Instance.CanSpawnEnemy())
            yield break;

        ChunkData data = GetComponentInParent<ChunkData>();
        if (data == null) yield break;
        float distanceFromPlayer = transform.parent.position.x - MapManager.Instance.player.position.x;

        int stage = GameManagers.Instance.stage;
        // 🔥 เงื่อนไขที่แกต้องการ
        if (stage < 2 || stage > 10)
            yield break;

        hasSpawned = true;

        int count = Mathf.Clamp(2 + stage, 4, 8);

        if (stage == 2)
            count = Random.Range(4, 7);   // 4–6
        else if (stage <= 5)
            count = Random.Range(4, 6);
        else if (stage <= 10)
            count = Random.Range(5, 8);

        // 🔥 กันทะลุ

        float speed = 3f + (stage - 1) * 0.7f;
        float playerX = MapManager.Instance.player.position.x;

        // 🔥 spawn ล่วงหน้า player
        float startX = playerX + 15f;

        for (int i = 0; i < count; i++)
        {
            if (!EnemyManager.Instance.CanSpawn())
                yield break;

            Debug.Log("SPAWN");
            float baseY = isTopLane
                ? MapManager.Instance.topY
                : MapManager.Instance.bottomY;

            float offset = isTopLane ? topOffset : bottomOffset;

            float y = baseY + offset;

            // y += groundOffset;

            Vector3 pos = new Vector3(
                startX + i * spacing + Random.Range(-1f, 1f),
                y,
                0
            );

            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);

            EnemyManager.Instance.RegisterEnemy(); // 🔥 สำคัญ

            if (isTopLane)
            {
                var sr = enemy.GetComponent<SpriteRenderer>();
                if (sr != null) sr.flipY = true;
            }

            var move = enemy.GetComponent<EnemyMove>();
            if (move != null) move.speed = speed;

            yield return new WaitForSeconds(0.4f);
        }
    }
}