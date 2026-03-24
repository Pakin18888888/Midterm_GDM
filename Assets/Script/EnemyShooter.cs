using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CombatConfig config;
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Optional")]
    [SerializeField] private Animator anim;
    [SerializeField] private string attackTriggerName = "attack";
    [SerializeField] private float shootDelay = 0.1f;
    [SerializeField] private float laneTolerance = 1.0f;
    [SerializeField] private EnemyLane enemyLane;
    [SerializeField] private PlayerMovement playerMovement;

    private float lastFireTime;
    private bool shooting;

    public bool IsShooting() => shooting;

    private void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (enemyLane == null)
            enemyLane = GetComponent<EnemyLane>();

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null || projectilePrefab == null || firePoint == null) return;

        if (IsPlayerInAttackRange())
        {
            TryShoot();
        }
    }

    public bool IsPlayerInAttackRange()
    {
        if (player == null || playerMovement == null || enemyLane == null) return false;

        float range = (config != null) ? config.attackRange : 2f;
        float dx = transform.position.x - player.position.x;

        bool sameLane = playerMovement.IsOnTopLane() == enemyLane.IsTopLane;

        return sameLane && dx >= 0f && dx <= range;
    }

    private void TryShoot()
    {
        float cooldown = (config != null) ? config.enemyAttackCooldown : 1f;
        if (Time.time < lastFireTime + cooldown) return;

        lastFireTime = Time.time;
        StartCoroutine(CoShoot());
    }

    private IEnumerator CoShoot()
    {
        
        shooting = true;

        if (anim != null && !string.IsNullOrEmpty(attackTriggerName))
            anim.SetTrigger(attackTriggerName);

        yield return new WaitForSeconds(shootDelay);
        AudioManager.I?.PlayEnemyShoot();

        GameObject obj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        EnemyProjectile proj = obj.GetComponent<EnemyProjectile>();
        if (proj != null)
        {
            proj.Setup(Vector2.left, transform.position, config);
        }

        yield return new WaitForSeconds(0.05f);
        shooting = false;
    }

    private void OnDrawGizmosSelected()
    {
        float range = (config != null) ? config.attackRange : 2f;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * range);
    }
}