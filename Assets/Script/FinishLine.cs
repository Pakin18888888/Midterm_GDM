using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool triggerOnce = true;

    private bool activated = false;

    private void Reset()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (c != null)
            c.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated && triggerOnce) return;
        if (!other.CompareTag(playerTag)) return;

        activated = true;
        GameManager.I?.Win();
    }
}