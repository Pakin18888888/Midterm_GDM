using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int value = 1;
    private Transform player;

    public float destroyDistance = 20f; // ระยะห่างหลัง player

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // 🔥 ถ้า coin อยู่ “หลัง player มากเกิน”
        if (transform.position.x < player.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (gameObject.CompareTag("Coin"))
        {
            ScoreManager.Instance.AddScore(1);
        }
        else if (gameObject.CompareTag("Magnet"))
        {
            other.GetComponent<MagnetEffect>().Activate(5f); // 5 วิ
        }

        Destroy(gameObject);
    }
}