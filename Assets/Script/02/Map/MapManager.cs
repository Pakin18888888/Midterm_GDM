using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public Transform player;

    public int chunkCount = 5;
    public float chunkLength = 20f;

    public float bottomY = -4.63f;
    public float topY = -2.5f;
    public int currentStage = 1;

    private List<GameObject> chunks = new List<GameObject>();
    private float spawnX = 0;

    void Start()
    {
        for (int i = 0; i < chunkCount; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        currentStage = GameManagers.Instance.stage;

        if (player.position.x > spawnX - (chunkLength * 20))
        {
            SpawnChunk();
            DeleteChunk();
        }
    }

    void SpawnChunk()
    {
        // 🔻 พื้นล่าง (มีตลอด)
        GameObject bottom = Instantiate(
            chunkPrefab,
            new Vector3(spawnX, bottomY, 0),
            Quaternion.identity
        );

        chunks.Add(bottom);

        // 🔺 พื้นบน (มีเฉพาะด่าน 2)
        if (currentStage >= 2)
        {
            GameObject top = Instantiate(
                chunkPrefab,
                new Vector3(spawnX, topY, 0),
                Quaternion.Euler(0, 0, 180)
            );

            chunks.Add(top);
        }

        spawnX += chunkLength;
    }

    void DeleteChunk()
    {
        int maxChunk = currentStage >= 2 ? chunkCount * 2 : chunkCount;

        if (chunks.Count > maxChunk)
        {
            Destroy(chunks[0]);
            chunks.RemoveAt(0);
        }
    }
}