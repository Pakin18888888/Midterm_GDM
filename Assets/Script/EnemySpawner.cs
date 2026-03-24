using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Points")]
    [SerializeField] private Transform topSpawnPoint;
    [SerializeField] private Transform bottomSpawnPoint;

    [Header("Spawn Settings")]
    [SerializeField] private int startSpawnCount = 5;
    [SerializeField] private float spawnIntervalMin = 0.8f;
    [SerializeField] private float spawnIntervalMax = 1.3f;
    [SerializeField] private float minSpawnXDistance = 3.5f;

    private bool spawningStarted = false;
    private readonly List<GameObject> aliveEnemies = new();

    public void StartSpawning()
    {
        if (spawningStarted) return;

        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();

        spawningStarted = true;
        StartCoroutine(CoSpawnStartWave());
    }

    private IEnumerator CoSpawnStartWave()
    {
        for (int i = 0; i < startSpawnCount; i++)
        {
            TrySpawnEnemy();
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
        }
    }

    private void TrySpawnEnemy()
    {
        CleanupDeadEnemies();

        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        if (playerMovement == null) return;

        Transform spawnPoint = playerMovement.IsOnTopLane() ? topSpawnPoint : bottomSpawnPoint;
        if (spawnPoint == null) return;

        if (!CanSpawnInLane(spawnPoint.position.y, spawnPoint.position.x)) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        EnemyLane lane = enemy.GetComponent<EnemyLane>();
        if (lane != null)
        {
            bool spawnTop = playerMovement.IsOnTopLane();
            lane.SetLane(spawnTop);
        }

        EnemyPolarity polarity = enemy.GetComponent<EnemyPolarity>();
        if (polarity != null)
        {
            polarity.SetPolarity(Random.value < 0.5f
                ? MagnetPolarity.Positive
                : MagnetPolarity.Negative);
        }

        aliveEnemies.Add(enemy);
    }

    private bool CanSpawnInLane(float laneY, float spawnX)
    {
        for (int i = 0; i < aliveEnemies.Count; i++)
        {
            GameObject enemy = aliveEnemies[i];
            if (enemy == null) continue;

            if (Mathf.Abs(enemy.transform.position.y - laneY) < 0.5f)
            {
                if (Mathf.Abs(enemy.transform.position.x - spawnX) < minSpawnXDistance)
                    return false;
            }
        }

        return true;
    }

    private void CleanupDeadEnemies()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] == null)
                aliveEnemies.RemoveAt(i);
        }
    }
}