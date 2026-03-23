using UnityEngine;

[RequireComponent(typeof(EnemyPolarity))]
public class EnemyLife : MonoBehaviour, IDamageable
{
    [SerializeField] private CombatConfig config;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float knockbackForce = 6f;

    [Header("Runtime")]
    [SerializeField] private int currentHP;

    private EnemyPolarity enemyPolarity;

    private void Awake()
    {
        enemyPolarity = GetComponent<EnemyPolarity>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHP = (config != null) ? config.enemyMaxHP : 3;
    }

    public MagnetPolarity GetPolarity()
    {
        return enemyPolarity.GetPolarity();
    }

    public void TakeDamage(int amount, Vector2 fromPosition)
    {
        currentHP -= Mathf.Max(0, amount);

        Vector2 dir = ((Vector2)transform.position - fromPosition).normalized;

        if (rb != null)
            rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}