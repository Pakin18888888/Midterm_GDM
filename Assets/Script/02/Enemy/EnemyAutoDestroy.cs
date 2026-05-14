using UnityEngine;

public class EnemyAutoDestroy : MonoBehaviour
{
    Transform player;

    void Start()
    {
        player = MapManager.Instance.player;
    }

    void Update()
    {
        if (player == null) return;

        // ถ้าศัตรูหลุดไปหลัง player มากเกิน
        if (transform.position.x < player.position.x - 20f)
        {
            EnemyManager.Instance.UnregisterEnemy();
            Destroy(gameObject);
        }
    }
}