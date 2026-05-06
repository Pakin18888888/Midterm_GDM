using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] chunkPrefabs;       // พื้นหลัก
    public GameObject[] bottomPrefabs;      // พื้นล่างฃ
    public Sprite[] groundSprites;      // พื้นหลัก
    public Sprite[] bottomSprites;      // พื้นล่าง
    public Transform player;
    public static MapManager Instance;
    public int difficultyLevel;

    public ChunkType currentChunkType;

    public int chunkCount = 30;
    public float chunkLength = 1f;
    public int extraLayers = 3;   // จำนวนชั้นล่าง
    public float layerSpacing = 1f; // ระยะห่างแต่ละชั้น
    public float bottomY = -4.63f;
    public float topY = -2.5f;
    public int currentStage = 1;
    public int keepChunksBehind = 10;

    float lastSpawnTime = 0f;
    public float spawnCooldown = 2f; int activeEnemyChunks = 0;

    private List<GameObject> chunks = new List<GameObject>();
    private float spawnX = 0;
    bool hasSpawnedThisStage = false;

    public enum ChunkType
    {
        Coin,
        Enemy,
        Mix,
        Empty
    }

    void Start()
    {
        for (int i = 0; i < chunkCount; i++)
        {
            SpawnChunk();
        }

    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (currentStage != GameManagers.Instance.stage)
        {
            hasSpawnedThisStage = false;
        }

        currentStage = GameManagers.Instance.stage;
        difficultyLevel = Mathf.FloorToInt(currentStage * 0.5f);

        if (player.position.x > spawnX - (chunkLength * 20))
        {
            SpawnChunk();
            DeleteChunk();
        }
        activeEnemyChunks = 0;

        bool foundEnemyChunk = false;

        bool canSpawn = Time.time - lastSpawnTime >= spawnCooldown;

        foreach (GameObject chunk in chunks)
        {
            if (chunk == null) continue;

            ChunkData data = chunk.GetComponent<ChunkData>();
            if (data == null) continue;

            if (data.hasSpawnedEnemy) continue;

            if (data.chunkType != ChunkType.Enemy && data.chunkType != ChunkType.Mix)
                continue;

            float spawnDistance = 15f;

            if (player.position.x > chunk.transform.position.x - spawnDistance)
            {
                if (!canSpawn)
                    break;
                EnemySpawner spawner = chunk.GetComponentInChildren<EnemySpawner>();

                if (spawner != null)
                {
                    spawner.Spawn();
                    data.hasSpawnedEnemy = true;

                    lastSpawnTime = Time.time; // 🔥 สำคัญ
                    break; // 🔥 ตัวนี้สำคัญมาก!!!
                }
            }
        }
    }
    ChunkType GetRandomChunkType()
    {
        float r = Random.value;

        if (r < 0.3f) return ChunkType.Coin;
        if (r < 0.7f) return ChunkType.Enemy; // 🔥 เพิ่มโอกาส

        return ChunkType.Mix;
    }

    GameObject GetChunkPrefabByType(ChunkType type)
    {
        switch (type)
        {
            case ChunkType.Coin:
                return chunkPrefabs[0];
            case ChunkType.Enemy:
                return chunkPrefabs[1];
            case ChunkType.Mix:
                return chunkPrefabs[2];
            case ChunkType.Empty:
                return chunkPrefabs[3];
        }

        return chunkPrefabs[0];
    }

    GameObject GetBottomPrefab()
    {
        int index = Mathf.Clamp(currentStage - 1, 0, bottomPrefabs.Length - 1);
        return bottomPrefabs[index];
    }
    int chunkSpawnedCount = 0;
    void SpawnChunk()
    {
        chunkSpawnedCount++;

        currentChunkType = GetRandomChunkType();

        if (currentStage >= 2 && Random.value < 0.6f)
        {
            currentChunkType = ChunkType.Enemy;
        }

        GameObject mainPrefab = GetChunkPrefabByType(currentChunkType);
        GameObject bottomPrefab = GetBottomPrefab();

        GameObject bottom = Instantiate(
            mainPrefab,
            new Vector3(spawnX, bottomY, 0),
            Quaternion.identity
        );

        SpriteRenderer sr = bottom.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            int index = (currentStage - 1) % groundSprites.Length;
            sr.sprite = groundSprites[index];
        }

        ChunkData data = bottom.AddComponent<ChunkData>();
        data.chunkType = currentChunkType;

        chunks.Add(bottom);


        // 🔻 พื้นล่างเพิ่ม (ต่ำลงไปอีก)
        for (int i = 1; i <= extraLayers; i++)
        {
            GameObject extraBottom = Instantiate(
                bottomPrefab,
                new Vector3(
                    spawnX,
                    bottomY - (layerSpacing * i), // 🔥 ลงทีละชั้น
                    0
                ),
                Quaternion.identity
            );
            SpriteRenderer sr2 = extraBottom.GetComponent<SpriteRenderer>();

            if (sr2 != null)
            {
                int index = (currentStage - 1) % bottomSprites.Length;
                sr2.sprite = bottomSprites[index];
            }

            chunks.Add(extraBottom);
        }

        // ItemSpawner bottomSpawner = bottom.GetComponentInChildren<ItemSpawner>();
        // if (bottomSpawner != null)
        // {
        //     bottomSpawner.isTopLane = false;
        // }
        EnemySpawner bottomEnemy = bottom.GetComponentInChildren<EnemySpawner>();
        if (bottomEnemy != null)
        {
            bottomEnemy.isTopLane = false;
        }
        // 🔺 พื้นบน (ด่าน 2)
        if (currentStage >= 2)
        {
            GameObject top = Instantiate(
                mainPrefab,
                new Vector3(spawnX, topY, 0),
                Quaternion.Euler(0, 0, 180)
            );
            SpriteRenderer srTop = top.GetComponent<SpriteRenderer>();

            if (srTop != null)
            {
                int index = (currentStage - 1) % groundSprites.Length;
                srTop.sprite = groundSprites[index];
            }
            ChunkData topData = top.AddComponent<ChunkData>();
            topData.chunkType = currentChunkType;
            topData.spawnStage = GameManagers.Instance.stage;
            chunks.Add(top);

            // ItemSpawner topSpawner = top.GetComponentInChildren<ItemSpawner>();
            // if (topSpawner != null)
            // {
            //     topSpawner.isTopLane = true;
            // }

            EnemySpawner topEnemy = top.GetComponentInChildren<EnemySpawner>();
            if (topEnemy != null)
            {
                topEnemy.isTopLane = true;
            }

            for (int i = 1; i <= extraLayers; i++)
            {
                GameObject extratop = Instantiate(
                bottomPrefab,
                new Vector3(
                    spawnX,
                    topY + (layerSpacing * i), 0),
                Quaternion.identity
            );
                SpriteRenderer srTop2 = extratop.GetComponent<SpriteRenderer>();
                if (srTop2 != null)
                {
                    int index = (currentStage - 1) % bottomSprites.Length;
                    srTop2.sprite = bottomSprites[index];
                }
                chunks.Add(extratop);

            }

        }

        spawnX += chunkLength;
    }

    void DeleteChunk()
    {
        float deleteX = player.position.x - (chunkLength * keepChunksBehind);

        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            if (chunks[i].transform.position.x < deleteX)
            {
                Destroy(chunks[i]);
                chunks.RemoveAt(i);
            }
        }
    }
}