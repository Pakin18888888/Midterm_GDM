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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (triggered) return;
        triggered = true;

        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position, volume);

        PlayerRoot playerRoot = other.GetComponent<PlayerRoot>();
        if (playerRoot != null)
            playerRoot.OnDeath();

        GameFlowController.I?.GameOver();

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, destroyDelay);
    }
}