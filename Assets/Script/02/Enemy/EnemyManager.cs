using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public int currentEnemyCount = 0;
    public int maxEnemy = 10;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Debug.Log("Enemy Count: " + currentEnemyCount);
    }

    public bool CanSpawn()
    {
        return currentEnemyCount < maxEnemy;
    }

    public void RegisterEnemy()
    {
        currentEnemyCount++;
    }

    public void UnregisterEnemy()
    {
        currentEnemyCount--;
    }
}