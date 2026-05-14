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

    public Material particleMaterial;

    float lastSpawnTime = 0f;
    public float spawnCooldown = 2f; int activeEnemyChunks = 0;

    private List<GameObject> chunks = new List<GameObject>();
    private float spawnX = 0;
    bool hasSpawnedThisStage = false;

    float nextWaveTime = 0f;
    bool waveActive = false;

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

        if (Time.time >= nextWaveTime)
        {
            SpawnEnemyWave();
        }
    }

    void SpawnEnemyWave()
    {
        if (!GameManagers.Instance.CanSpawnEnemy())
            return;

        if (GameManagers.Instance.isRestStage)
            return;

        if (!GameManagers.Instance.usePolarity)
            return;

        waveActive = true;

        nextWaveTime =
            Time.time + Random.Range(5f, 9f);

        // หา chunk ข้างหน้า player
        foreach (GameObject chunk in chunks)
        {
            if (chunk == null)
                continue;

            float distance =
                chunk.transform.position.x
                - player.position.x;

            if (distance < 15f || distance > 25f)
                continue;

            EnemySpawner[] spawners =
                chunk.GetComponentsInChildren<EnemySpawner>();

            // ===== Stage 1-4 =====
            // spawn lane เดียว
            if (currentStage < 5)
            {
                bool spawnTopLane =
                    Random.value > 0.5f;

                foreach (EnemySpawner spawner in spawners)
                {
                    if (spawner != null)
                    {
                        spawner.Spawn(spawnTopLane);
                    }
                }
            }

            // ===== Stage 5+ =====
            // spawn ทั้งบน+ล่าง
            else
            {
                foreach (EnemySpawner spawner in spawners)
                {
                    if (spawner != null)
                    {
                        // ล่าง
                        spawner.Spawn(false);

                        // บน
                        spawner.Spawn(true);
                    }
                }
            }

            break;
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

        // ===== polarity ของ chunk นี้ =====

        int chunkStage = GameManagers.Instance.stage;

        bool flip = chunkStage % 2 == 0;

        PolarityType topPolarity;
        PolarityType bottomPolarity;

        if (flip)
        {
            topPolarity = PolarityType.Positive;
            bottomPolarity = PolarityType.Negative;
        }
        else
        {
            topPolarity = PolarityType.Negative;
            bottomPolarity = PolarityType.Positive;
        }

        // ===== enemy chance =====

        if (chunkStage >= 2)
        {
            float enemyChance = Mathf.Clamp(
                chunkStage * 0.08f,
                0.15f,
                0.45f
            );

            if (Random.value < enemyChance)
            {
                currentChunkType = ChunkType.Enemy;
            }
        }

        GameObject mainPrefab =
            GetChunkPrefabByType(currentChunkType);

        GameObject bottomPrefab =
            GetBottomPrefab();

        // =====================================================
        // ================= BOTTOM ============================
        // =====================================================

        GameObject bottom = Instantiate(
            mainPrefab,
            new Vector3(spawnX, bottomY, 0),
            Quaternion.identity
        );

        LanePolarity bottomLane =
            bottom.AddComponent<LanePolarity>();

        bottomLane.lanePolarity =
            bottomPolarity;

        bottomLane.stage = chunkStage;
        LaneVisuals bottomVisual =
            bottom.AddComponent<LaneVisuals>();

        bottomVisual.particleMaterial =
            particleMaterial;

        SpriteRenderer sr =
            bottom.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            int index =
                (currentStage - 1)
                % groundSprites.Length;

            sr.sprite =
                groundSprites[index];
        }

        ChunkData data =
            bottom.AddComponent<ChunkData>();

        data.usePolarity =
            GameManagers.Instance.usePolarity;

        data.topPolarity =
            topPolarity;

        data.bottomPolarity =
            bottomPolarity;

        data.chunkType =
            currentChunkType;

        data.stage = chunkStage;

        chunks.Add(bottom);

        // =====================================================
        // =============== EXTRA BOTTOM ========================
        // =====================================================

        for (int i = 1; i <= extraLayers; i++)
        {
            GameObject extraBottom = Instantiate(
                bottomPrefab,
                new Vector3(
                    spawnX,
                    bottomY - (layerSpacing * i),
                    0
                ),
                Quaternion.identity
            );

            SpriteRenderer sr2 =
                extraBottom.GetComponent<SpriteRenderer>();

            if (sr2 != null)
            {
                int index =
                    (currentStage - 1)
                    % bottomSprites.Length;

                sr2.sprite =
                    bottomSprites[index];
            }

            chunks.Add(extraBottom);
        }

        // =====================================================
        // ==================== TOP ============================
        // =====================================================

        if (currentStage >= 2)
        {
            GameObject top = Instantiate(
                mainPrefab,
                new Vector3(spawnX, topY, 0),
                Quaternion.Euler(0, 0, 180)
            );

            SpriteRenderer srTop =
                top.GetComponent<SpriteRenderer>();

            if (srTop != null)
            {
                int index =
                    (currentStage - 1)
                    % groundSprites.Length;

                srTop.sprite =
                    groundSprites[index];
            }

            LanePolarity topLane =
                top.AddComponent<LanePolarity>();

            topLane.lanePolarity =
                topPolarity;

            topLane.stage = chunkStage;

            LaneVisuals topVisual =
                top.AddComponent<LaneVisuals>();

            topVisual.particleMaterial =
                particleMaterial;

            ChunkData topData =
                top.AddComponent<ChunkData>();

            topData.usePolarity =
                GameManagers.Instance.usePolarity;

            topData.topPolarity =
                topPolarity;

            topData.bottomPolarity =
                bottomPolarity;

            topData.chunkType =
                currentChunkType;

            topData.stage =
                chunkStage;

            topData.spawnStage =
                GameManagers.Instance.stage;

            chunks.Add(top);

            // =================================================
            // ============== EXTRA TOP ========================
            // =================================================

            for (int i = 1; i <= extraLayers; i++)
            {
                GameObject extraTop = Instantiate(
                    bottomPrefab,
                    new Vector3(
                        spawnX,
                        topY + (layerSpacing * i),
                        0
                    ),
                    Quaternion.identity
                );

                SpriteRenderer srTop2 =
                    extraTop.GetComponent<SpriteRenderer>();

                if (srTop2 != null)
                {
                    int index =
                        (currentStage - 1)
                        % bottomSprites.Length;

                    srTop2.sprite =
                        bottomSprites[index];
                }

                chunks.Add(extraTop);
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