using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int count = 10;
    public float spacing = 1.5f;
    public float yOffset = 1f;

    public bool isTopLane = false; // 🔥 เพิ่ม

    void Start()
    {
        if (!GameManagers.Instance.CanSpawnCoin())
            return;

        float startX = transform.parent.position.x;

        float offset = isTopLane ? -yOffset : yOffset;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(
                startX + i * spacing,
                transform.parent.position.y + offset,
                0
            );

            Instantiate(coinPrefab, pos, Quaternion.identity, transform.parent);
        }
    }
}