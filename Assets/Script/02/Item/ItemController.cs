using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int value = 1;

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