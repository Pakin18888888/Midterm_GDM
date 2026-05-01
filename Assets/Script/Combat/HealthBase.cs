using UnityEngine;

public abstract class HealthBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHP = 3;
    [SerializeField] protected int currentHP;

    protected virtual void Awake()
    {
        currentHP = maxHP;
    }

    public virtual void TakeDamage(int amount, Vector2 fromPosition)
    {
        currentHP -= Mathf.Max(0, amount);

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    protected abstract void Die();
}