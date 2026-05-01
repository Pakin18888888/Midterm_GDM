using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Vector2 fromPosition;
    private int damage = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector2 direction, Vector2 attackerPosition, CombatConfig config)
    {
        moveDir = direction.normalized;
        fromPosition = attackerPosition;
        damage = (config != null) ? config.enemyDamage : 1;

        rb.linearVelocity = moveDir * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null)
                dmg.TakeDamage(damage, fromPosition);

            Destroy(gameObject);
            return;
        }

        if (!other.isTrigger)
            Destroy(gameObject);
    }
}