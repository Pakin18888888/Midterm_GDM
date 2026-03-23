using UnityEngine;

public class CollectibleCoin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private float volume = 1f;

    void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c != null)
            c.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        GameManager.I?.AddCoin(value);

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);

        gameObject.SetActive(false);
    }
}