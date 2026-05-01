// using UnityEngine;
// using System.Collections;

// public class EnemyCombat : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private CombatConfig config;
//     [SerializeField] private Transform player;
//     [SerializeField] private PlayerRoot playerRoot;
//     [SerializeField] private Transform firePoint;
//     [SerializeField] private EnemyProjectile projectilePrefab;
//     [SerializeField] private EnemyLaneInfo laneInfo;

//     [Header("Shoot Delay")]
//     [SerializeField] private float preShootDelay = 0.1f;

//     private float lastFireTime;
//     private bool shooting;

//     public bool IsShooting => shooting;

//     private void Start()
//     {
//         if (player == null || playerRoot == null)
//         {
//             GameObject p = GameObject.FindGameObjectWithTag("Player");
//             if (p != null)
//             {
//                 if (player == null)
//                     player = p.transform;

//                 if (playerRoot == null)
//                     playerRoot = p.GetComponent<PlayerRoot>();
//             }
//         }

//         if (laneInfo == null)
//             laneInfo = GetComponent<EnemyLaneInfo>();
//     }

//     private void Update()
//     {
//         if (CanShootPlayer())
//             TryShoot();
//     }

//     public bool CanShootPlayer()
//     {
//         if (player == null || playerRoot == null || laneInfo == null)
//             return false;

//         float range = config != null ? config.attackRange : 2f;
//         float dx = transform.position.x - player.position.x;
//         bool sameLane = playerRoot.IsOnTopLane == laneInfo.IsTopLane;

//         return sameLane && dx >= 0f && dx <= range;
//     }

//     private void TryShoot()
//     {
//         if (shooting) return;

//         float cooldown = config != null ? config.enemyAttackCooldown : 1f;
//         if (Time.time < lastFireTime + cooldown) return;

//         if (projectilePrefab == null || firePoint == null) return;

//         lastFireTime = Time.time;
//         StartCoroutine(CoShoot());
//     }

//     private IEnumerator CoShoot()
//     {
//         shooting = true;

//         yield return new WaitForSeconds(preShootDelay);

//         if (projectilePrefab != null && firePoint != null)
//         {
//             Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
//             AudioManager.I?.PlayEnemyShoot();
//         }

//         shooting = false;
//     }
// }