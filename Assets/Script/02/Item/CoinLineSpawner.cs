using UnityEngine;

public class CoinLineSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int count = 10;
    public float spacing = 1.5f;
    public float yOffset = 1f;

    void Start()
    {
        SpawnLine();
    }

    void SpawnLine()
    {
        Vector3 startPos = transform.position;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(
                startPos.x + i * spacing,
                startPos.y + yOffset,
                0
            );

            Instantiate(coinPrefab, pos, Quaternion.identity, transform);
        }
    }
}