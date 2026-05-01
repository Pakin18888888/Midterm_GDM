using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damage = 1;
    public float hitCooldown = 0.8f;
    private bool canHit = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canHit) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerHealths>();
            if (player != null)
            {
                player.TakeDamage(damage);
                StartCoroutine(Cooldown());
            }
        }
    }

    System.Collections.IEnumerator Cooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(hitCooldown);
        canHit = true;
    }
}