using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    public float spawnDistance = 25f;

    public float minDelay = 2f;
    public float maxDelay = 4f;

    public int minCount = 2;
    public int maxCount = 4;

    public float[] lanes = { -4.63f, -2.5f }; // lane ล่าง / บน

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(3f); // delay ตอนเริ่ม

        while (true)
        {
            if (!GameManagers.Instance.CanSpawnEnemy())
            {
                yield return null;
                continue;
            }

            SpawnGroup();

            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    void SpawnGroup()
    {
        int count = Random.Range(minCount, maxCount + 1);

        float baseX = player.position.x + spawnDistance;

        for (int i = 0; i < count; i++)
        {
            float laneY = lanes[Random.Range(0, lanes.Length)];

            Vector3 pos = new Vector3(
                baseX + i * 2f, // เรียงกัน
                laneY,
                0
            );

            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }
}