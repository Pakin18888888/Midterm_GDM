// using UnityEngine;

// public class EnemyHealth : HealthBase
// {
//     [SerializeField] private Rigidbody2D rb;
//     [SerializeField] private float knockbackForce = 6f;
//     [SerializeField] private GameObject dropItemPrefab;
//     [SerializeField] private int dropCount = 1;

//     public override void TakeDamage(int amount, Vector2 fromPosition)
//     {
//         base.TakeDamage(amount, fromPosition);

//         if (currentHP > 0 && rb != null)
//         {
//             Vector2 dir = ((Vector2)transform.position - fromPosition).normalized;
//             rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
//         }
//     }

//     protected override void Die()
//     {
//         if (dropItemPrefab != null)
//         {
//             for (int i = 0; i < dropCount; i++)
//             {
//                 Vector3 pos = transform.position + new Vector3(i * 0.2f, 0f, 0f);
//                 // Instantiate(dropItemPrefab, pos, Quaternion.identity);
//             }
//         }

//         ScoreManager.Instance?.AddKill();
//         Destroy(gameObject);
//     }
// }