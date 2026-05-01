// using UnityEngine;
// using UnityEngine.InputSystem;

// public class TouchInputReader : MonoBehaviour
// {
//     private PlayerCon inputActions;
//     private Vector2 startTouchPosition;
//     private Vector2 endTouchPosition;

//     [Header("References")]
//     [SerializeField] private Transform swipeTrailObject;
//     [SerializeField] private TrailRenderer swipeTrail;
//     [SerializeField] private Camera mainCamera;
//     [SerializeField] private PlayerRoot playerRoot;

//     [Header("Swipe Settings")]
//     [SerializeField] private float minSwipeDistance = 50f;

//     private void Awake()
//     {
//         inputActions = new PlayerCon();

//         if (mainCamera == null)
//             mainCamera = Camera.main;

//         if (playerRoot == null)
//             playerRoot = FindFirstObjectByType<PlayerRoot>();
//     }

//     private void Start()
//     {
//         if (swipeTrail != null)
//             swipeTrail.gameObject.SetActive(false);
//     }

//     private void Update()
//     {
//         bool isTouching = inputActions.Touch.PrimaryContact.IsPressed();

//         if (isTouching && swipeTrailObject != null && swipeTrailObject.gameObject.activeSelf)
//         {
//             Vector2 currentTouchPosition = inputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
//             Vector3 currentWorld = ScreenToWorldPoint(currentTouchPosition);
//             swipeTrailObject.position = currentWorld;
//         }
//     }

//     private void OnEnable()
//     {
//         inputActions.Enable();

//         inputActions.Touch.PrimaryContact.started += OnTouchStarted;
//         inputActions.Touch.PrimaryContact.canceled += OnTouchEnded;
//     }

//     private void OnDisable()
//     {
//         inputActions.Touch.PrimaryContact.started -= OnTouchStarted;
//         inputActions.Touch.PrimaryContact.canceled -= OnTouchEnded;

//         inputActions.Disable();
//     }

//     private void OnTouchStarted(InputAction.CallbackContext context)
//     {
//         startTouchPosition = inputActions.Touch.PrimaryPosition.ReadValue<Vector2>();

//         if (swipeTrail != null)
//             swipeTrail.Clear();

//         if (swipeTrailObject != null)
//         {
//             Vector3 startWorld = ScreenToWorldPoint(startTouchPosition);
//             swipeTrailObject.position = startWorld;
//             swipeTrailObject.gameObject.SetActive(true);
//         }
//     }

//     private void OnTouchEnded(InputAction.CallbackContext context)
//     {
//         endTouchPosition = inputActions.Touch.PrimaryPosition.ReadValue<Vector2>();

//         if (swipeTrailObject != null)
//             swipeTrailObject.gameObject.SetActive(false);

//         DetectSwipe();
//     }

//     private void DetectSwipe()
//     {
//         if (playerRoot == null) return;

//         Vector2 swipeDelta = endTouchPosition - startTouchPosition;

//         if (swipeDelta.magnitude < minSwipeDistance)
//         {
//             Debug.Log("Swipe too short");
//             return;
//         }

//         if (Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x))
//         {
//             Debug.Log("Vertical Swipe -> Jump Lane");
//             playerRoot.JumpLane();
//         }
//         else
//         {
//             if (swipeDelta.x > 0)
//             {
//                 Debug.Log("Swipe Right -> Attack");
//                 // playerRoot.Attack();
//             }
//         }
//     }

//     private Vector3 ScreenToWorldPoint(Vector2 screenPosition)
//     {
//         Vector3 screenPoint = new Vector3(screenPosition.x, screenPosition.y, 10f);
//         Vector3 worldPoint = mainCamera.ScreenToWorldPoint(screenPoint);
//         worldPoint.z = 0f;
//         return worldPoint;
//     }
// }