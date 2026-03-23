using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PushProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 1.5f;
    [SerializeField] private float pushForce = 8f;
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody2D rb;
    private int damage;
    private Vector2 ownerPosition;
    private Vector2 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(int attackDamage, Vector2 direction, Vector2 fromPosition)
    {
        damage = attackDamage;
        moveDir = direction.normalized;
        ownerPosition = fromPosition;

        rb.linearVelocity = moveDir * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) == 0)
            return;

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, ownerPosition);
        }

        Rigidbody2D enemyRb = other.attachedRigidbody;
        if (enemyRb != null)
        {
            enemyRb.AddForce(moveDir * pushForce, ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }
}