using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float destroyDelay = 0.05f;

    [Header("Sound")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private float volume = 1f;

    private bool triggered;

    private void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c != null)
            c.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (triggered) return;
        triggered = true;

        // 🔊 เล่นเสียงก่อนลบ
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position, volume);

        // 💀 ทำให้ผู้เล่นตาย
        var life = other.GetComponent<PlayerLife>();
        if (life != null)
            life.Kill();
        else
            GameManager.I?.PlayerDied();

        // ปิด collider กันชนซ้ำ
        GetComponent<Collider2D>().enabled = false;

        // ลบหนาม
        Destroy(gameObject, destroyDelay);
    }
}