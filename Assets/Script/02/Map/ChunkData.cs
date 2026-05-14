using UnityEngine;

public class ChunkData : MonoBehaviour
{
    public MapManager.ChunkType chunkType;

    public bool hasSpawnedEnemy = false;

    public int spawnStage;

    public bool usePolarity;

    public PolarityType topPolarity;

    public PolarityType bottomPolarity;
    public int stage;
}