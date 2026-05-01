using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private CombatConfig config;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PushProjectile projectilePrefab;
    [SerializeField] private PlayerPolarity polarity;

    private bool canControl = true;
    private float lastAttackTime;

    public void SetControlEnabled(bool enabled) => canControl = enabled;

    public void TryAttack()
    {
        if (!canControl) return;

        float cooldown = config != null ? config.playerAttackCooldown : 0.5f;
        if (Time.time < lastAttackTime + cooldown) return;

        if (firePoint == null || projectilePrefab == null || polarity == null) return;

        lastAttackTime = Time.time;

        var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.Setup(
            config != null ? config.playerDamage : 1,
            transform.right,
            transform.position,
            polarity.CurrentPolarity,
            config
        );

        AudioManager.I?.PlayPlayerShoot();
    }
}