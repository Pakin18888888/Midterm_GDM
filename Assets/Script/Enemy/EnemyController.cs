// using UnityEngine;

// public class EnemyController : MonoBehaviour
// {
//     [SerializeField] private EnemyMovement movement;
//     [SerializeField] private EnemyCombat combat;
//     [SerializeField] private EnemyHealth health;
//     [SerializeField] private EnemyLaneInfo laneInfo;
//     [SerializeField] private EnemyPolarity polarity;

//     public bool IsOnTopLane => laneInfo != null && laneInfo.IsTopLane;
//     public MagnetPolarity CurrentPolarity => polarity != null ? polarity.GetPolarity() : MagnetPolarity.Positive;
//     public void SetActiveAI(bool active)
//     {
//         if (movement != null)
//             movement.enabled = active;

//         if (combat != null)
//             combat.enabled = active;

//         // ถ้ามี Rigidbody ให้หยุดด้วย
//         Rigidbody2D rb = GetComponent<Rigidbody2D>();
//         if (rb != null)
//             rb.linearVelocity = Vector2.zero;
//     }
// }