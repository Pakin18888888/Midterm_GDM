using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PushProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 1.5f;
    [SerializeField] private float defaultPushForce = 8f;
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody2D rb;
    private int damage;
    private Vector2 ownerPosition;
    private Vector2 moveDir;
    private MagnetPolarity projectilePolarity;
    private CombatConfig config;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(
        int attackDamage,
        Vector2 direction,
        Vector2 fromPosition,
        MagnetPolarity polarity,
        CombatConfig combatConfig)
    {
        damage = attackDamage;
        moveDir = direction.normalized;
        ownerPosition = fromPosition;
        projectilePolarity = polarity;
        config = combatConfig;

        if (rb != null)
            rb.linearVelocity = moveDir * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) == 0)
            return;

        EnemyPolarity enemyPolarity = other.GetComponent<EnemyPolarity>();
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyPolarity == null || enemyHealth == null)
        {
            Destroy(gameObject);
            return;
        }

        bool samePolarity = enemyPolarity.IsSamePolarity(projectilePolarity);

        if (samePolarity)
        {
            enemyHealth.TakeDamage(damage, ownerPosition);

            Rigidbody2D enemyRb = other.attachedRigidbody;
            if (enemyRb != null)
            {
                float pushForce = (config != null) ? config.playerDamage * 4f : defaultPushForce;
                enemyRb.AddForce(moveDir * pushForce, ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject);
    }
}