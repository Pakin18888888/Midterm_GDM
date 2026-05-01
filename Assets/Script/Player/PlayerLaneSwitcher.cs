// using UnityEngine;
// using System.Collections;

// public class PlayerLaneSwitcher : MonoBehaviour
// {
//     [Header("Lane References")]
//     [SerializeField] private MagnetLane topLane;
//     [SerializeField] private MagnetLane bottomLane;
//     [SerializeField] private PlayerPolarity polarity;
//     [SerializeField] private PlayerRoot playerRoot;

//     [Header("Move Settings")]
//     [SerializeField] private float switchDuration = 0.2f;
//     [SerializeField] private float repelHoldTime = 0.15f;
//     [SerializeField] private float repelHeightPercent = 0.65f;

//     [Header("Visual")]
//     [SerializeField] private Transform visualRoot;

//     private bool canControl = true;
//     private bool switching;
//     private bool isOnTopLane;

//     public bool IsOnTopLane => isOnTopLane;
//     public bool IsSwitching => switching;

//     private void Awake()
//     {
//         if (polarity == null)
//             polarity = GetComponent<PlayerPolarity>();

//         if (visualRoot == null)
//             visualRoot = transform;

//         if (playerRoot == null)
//             playerRoot = GetComponent<PlayerRoot>();
//     }

//     private void Start()
//     {
//         ApplyLaneVisualInstant();
//     }

//     public void SetControlEnabled(bool enabled)
//     {
//         canControl = enabled;
//     }

//     public void TrySwitchLane()
//     {
//         if (!canControl || switching) return;
//         if (topLane == null || bottomLane == null) return;

//         AudioManager.I?.PlayPlayerJump();
//         StartCoroutine(CoSwitchLaneNormal());
//     }

//     public void ForceMoveToTopLane()
//     {
//         ForceMoveToLane(true);
//     }

//     public void ForceMoveToBottomLane()
//     {
//         ForceMoveToLane(false);
//     }

//     public void ForceMoveToLane(bool toTop)
//     {
//         if (switching) return;
//         if (topLane == null || bottomLane == null) return;
//         if (isOnTopLane == toTop) return;

//         AudioManager.I?.PlayPlayerJump();
//         StartCoroutine(CoForceMoveToLane(toTop));
//     }

//     private IEnumerator CoSwitchLaneNormal()
//     {
//         switching = true;

//         MagnetLane currentLane = isOnTopLane ? topLane : bottomLane;
//         MagnetLane targetLane = isOnTopLane ? bottomLane : topLane;

//         float currentY = currentLane.AttachPoint.position.y;
//         float targetY = targetLane.AttachPoint.position.y;

//         bool canAttach = true;

//         if (polarity != null)
//             canAttach = targetLane.CanAttach(polarity.CurrentPolarity);

//         if (canAttach)
//         {
//             yield return MoveToY(targetY, switchDuration);

//             isOnTopLane = !isOnTopLane;
//             ApplyLaneVisualInstant();
//         }
//         else
//         {
//             float repelY = Mathf.Lerp(currentY, targetY, repelHeightPercent);

//             yield return MoveToY(repelY, switchDuration * 0.8f);
//             yield return new WaitForSeconds(repelHoldTime);
//             yield return MoveToY(currentY, switchDuration * 0.8f);

//             ApplyLaneVisualInstant();
//         }

//         switching = false;
//         playerRoot?.EndJump();
//     }

//     private IEnumerator CoForceMoveToLane(bool toTop)
//     {
//         switching = true;

//         MagnetLane targetLane = toTop ? topLane : bottomLane;
//         float targetY = targetLane.AttachPoint.position.y;

//         yield return MoveToY(targetY, switchDuration);

//         isOnTopLane = toTop;
//         ApplyLaneVisualInstant();

//         switching = false;
//     }

//     private IEnumerator MoveToY(float targetY, float duration)
//     {
//         float startY = transform.position.y;
//         float t = 0f;

//         while (t < duration)
//         {
//             t += Time.deltaTime;
//             float p = Mathf.Clamp01(t / duration);

//             Vector3 pos = transform.position;
//             pos.y = Mathf.Lerp(startY, targetY, p);
//             transform.position = pos;

//             yield return null;
//         }

//         Vector3 finalPos = transform.position;
//         finalPos.y = targetY;
//         transform.position = finalPos;
//     }

//     private void ApplyLaneVisualInstant()
//     {
//         if (visualRoot == null) return;

//         Vector3 scale = visualRoot.localScale;

//         // หันขวาเสมอ
//         scale.x = Mathf.Abs(scale.x);

//         // เลนบน = กลับหัว, เลนล่าง = ปกติ
//         scale.y = isOnTopLane ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);

//         visualRoot.localScale = scale;
//     }
// }