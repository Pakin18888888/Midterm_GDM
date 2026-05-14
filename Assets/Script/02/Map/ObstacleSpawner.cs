using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    Transform player;

    float nextSpawnX = 30f;

    void Start()
    {
        player = MapManager.Instance.player;
    }

    void Update()
    {
        if (GameManagers.Instance.stage < 2 || GameManagers.Instance.isRestStage)
        {
            return;
        }

        if (player == null) return;

        if (player.position.x + 30f > nextSpawnX)
        {
            SpawnObstacle();

            nextSpawnX += Random.Range(18f, 30f);
        }
    }

    void SpawnObstacle()
    {
        int stage = GameManagers.Instance.stage;

        int index = Random.Range(0, obstaclePrefabs.Length);

        float y =
            Random.value > 0.5f
            ? MapManager.Instance.bottomY + 1f
            : MapManager.Instance.topY - 1f;

        Vector3 pos = new Vector3(
            nextSpawnX,
            y,
            0
        );

        GameObject obj = Instantiate(
    obstaclePrefabs[index],
    pos,
    Quaternion.identity
);

        obj.AddComponent<ObstacleAutoDestroy>();
    }
}