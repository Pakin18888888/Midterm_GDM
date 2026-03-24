using UnityEngine;

public class CollectibleCoin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float moveSpeed = 6f;
    private Transform player;
    private bool isCollected = false;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (isCollected && player != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c != null)
            c.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        isCollected = true;

        GameManager.I?.AddCoin(value);
        GameManager.I?.ShowMessage("+1", true);

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);

        Destroy(gameObject, 0.1f);
    }
}