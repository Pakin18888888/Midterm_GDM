using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private CombatConfig config;

    private Rigidbody2D rb;
    private EnemyShooter shooter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shooter = GetComponent<EnemyShooter>();
    }

    private void FixedUpdate()
    {
        if (shooter != null && shooter.IsPlayerInAttackRange())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float speed = (config != null) ? config.patrolSpeed : 2f;
        rb.linearVelocity = new Vector2(-speed, 0f);
    }
}