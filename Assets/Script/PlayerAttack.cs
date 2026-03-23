using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private CombatConfig config;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject pushProjectilePrefab;

    private float lastAttackTime;

    public void TryAttack()
    {
        float cd = (config != null) ? config.playerAttackCooldown : 0.5f;
        if (Time.time < lastAttackTime + cd) return;
        lastAttackTime = Time.time;

        if (pushProjectilePrefab == null || firePoint == null) return;

        GameObject obj = Instantiate(pushProjectilePrefab, firePoint.position, firePoint.rotation);

        PushProjectile projectile = obj.GetComponent<PushProjectile>();
        if (projectile != null)
        {
            int damage = (config != null) ? config.playerDamage : 1;
            projectile.Setup(damage, transform.right, transform.position);
        }
    }
}