using UnityEngine;

public class ObstaclePenalty : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // รี streak
        ScoreManager.Instance.streak = 0;

        // เอฟเฟกต์
        ScreenFlash.Instance.Flash();

        CameraShakes.Instance.Shake(0.15f, 2f);

        // เด้งกลับ
        other.transform.position += Vector3.left * 1.5f;
    }
}