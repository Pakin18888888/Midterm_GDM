// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class EnemySpawner : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private PlayerRoot playerRoot;

//     [Header("Enemy Prefabs")]
//     [SerializeField] private GameObject[] enemyPrefabs;

//     [Header("Spawn Points")]
//     [SerializeField] private Transform topSpawnPoint;
//     [SerializeField] private Transform bottomSpawnPoint;

//     [Header("Spawn Settings")]
//     [SerializeField] private int startSpawnCount = 5;
//     [SerializeField] private int maxAliveEnemies = 10;
//     [SerializeField] private float spawnIntervalMin = 0.8f;
//     [SerializeField] private float spawnIntervalMax = 1.3f;
//     [SerializeField] private float minSpawnXDistance = 3.5f;

//     private bool spawningStarted = false;
//     private readonly List<GameObject> aliveEnemies = new();

//     private Coroutine spawnRoutine;

//     public void StartSpawning()
//     {
//         Debug.Log("StartSpawning CALLED");

//         if (spawningStarted) return;
//         if (spawnRoutine != null) return; // กัน coroutine ซ้ำ

//         spawningStarted = true;
//         spawnRoutine = StartCoroutine(CoSpawnStartWave());
//     }

//     private IEnumerator CoSpawnStartWave()
//     {
//         for (int i = 0; i < startSpawnCount; i++)
//         {
//             TrySpawnEnemy();
//             yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
//         }
//     }

//     private void TrySpawnEnemy()
//     {
//         CleanupDeadEnemies();
//         if (aliveEnemies.Count >= maxAliveEnemies) return;

//         if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
//         if (playerRoot == null) return;

//         bool spawnTop = playerRoot.IsOnTopLane;
//         Transform spawnPoint = spawnTop ? topSpawnPoint : bottomSpawnPoint;
//         if (spawnPoint == null) return;

//         if (!CanSpawnInLane(spawnPoint.position.y, spawnPoint.position.x)) return;

//         GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
//         GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

//         EnemyLaneInfo lane = enemy.GetComponent<EnemyLaneInfo>();
//         if (lane != null)
//             lane.SetLane(spawnTop);

//         EnemyPolarity polarity = enemy.GetComponent<EnemyPolarity>();
//         if (polarity != null)
//         {
//             polarity.SetPolarity(Random.value < 0.5f
//                 ? MagnetPolarity.Positive
//                 : MagnetPolarity.Negative);
//         }

//         aliveEnemies.Add(enemy);
//     }

//     private bool CanSpawnInLane(float laneY, float spawnX)
//     {
//         for (int i = 0; i < aliveEnemies.Count; i++)
//         {
//             GameObject enemy = aliveEnemies[i];
//             if (enemy == null) continue;

//             if (Mathf.Abs(enemy.transform.position.y - laneY) < 0.5f)
//             {
//                 if (Mathf.Abs(enemy.transform.position.x - spawnX) < minSpawnXDistance)
//                     return false;
//             }
//         }

//         return true;
//     }

//     private void CleanupDeadEnemies()
//     {
//         for (int i = aliveEnemies.Count - 1; i >= 0; i--)
//         {
//             if (aliveEnemies[i] == null)
//                 aliveEnemies.RemoveAt(i);
//         }
//     }
// }