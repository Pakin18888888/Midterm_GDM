using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spacing = 2.5f;
    public int maxEnemyOnScreen = 5;
    float groundOffset = 0.8f; // ปรับเอา
    public float bottomOffset = 0.8f;
    public float topOffset = -0.5f;
    bool hasSpawnedTop;
    bool hasSpawnedBottom;

    public void Spawn(bool spawnTopLane)
    {
        if (spawnTopLane)
        {
            if (hasSpawnedTop)
                return;

            hasSpawnedTop = true;
        }
        else
        {
            if (hasSpawnedBottom)
                return;

            hasSpawnedBottom = true;
        }

        StartCoroutine(
            SpawnRoutine(spawnTopLane)
        );
    }

    IEnumerator SpawnRoutine(bool spawnTopLane)
    {
        if (!GameManagers.Instance.CanSpawnEnemy())
            yield break;

        int stage = GameManagers.Instance.stage;
        // ===== จำนวน =====

        int count = 2;

        if (stage >= 4) count = 3;
        if (stage >= 7) count = 4;
        if (stage >= 10) count = 5;

        // ===== ความเร็ว =====

        float speed = 3f + (stage * 0.25f);

        float playerX = MapManager.Instance.player.position.x;

        // spawn ไกลขึ้นหน่อย
        float startX = playerX + 20f;

        float y =
            spawnTopLane
            ? MapManager.Instance.topY + topOffset
            : MapManager.Instance.bottomY + bottomOffset ;


        // ===== pattern =====

        int pattern = 0;

        // ด่านแรกๆใช้แต่ LINE
        if (stage < 4)
        {
            pattern = 0;
        }
        else
        {
            pattern = Random.Range(0, 3);
        }

        for (int i = 0; i < count; i++)
        {
            if (!EnemyManager.Instance.CanSpawn())
                yield break;

            float x = startX;

            switch (pattern)
            {
                // ===== LINE =====
                case 0:

                    x += i * Random.Range(5f, 8f);

                    break;

                // ===== RANDOM =====
                case 2:

                    x += i * Random.Range(5f, 8f);

                    break;
            }

            Vector3 pos = new Vector3(x, y, 0);

            GameObject enemy = Instantiate(
                enemyPrefab,
                pos,
                spawnTopLane
                ? Quaternion.Euler(0, 0, 180)
                : Quaternion.identity
            );

            enemy.AddComponent<EnemyAutoDestroy>();

            EnemyManager.Instance.RegisterEnemy();

            var move = enemy.GetComponent<EnemyMove>();

            if (move != null)
            {
                move.speed = speed;
            }

            // ค่อยๆ spawn
            yield return new WaitForSeconds(
                Mathf.Clamp(
                    0.8f - stage * 0.03f,
                    0.25f,
                    0.8f
                )
            );
        }
    }
}