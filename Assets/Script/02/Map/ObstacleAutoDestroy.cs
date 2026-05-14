using UnityEngine;

public class ObstacleAutoDestroy : MonoBehaviour
{
    Transform player;

    void Start()
    {
        player = MapManager.Instance.player;
    }

    void Update()
    {
        if (player == null) return;

        if (transform.position.x < player.position.x - 25f)
        {
            Destroy(gameObject);
        }
    }
}