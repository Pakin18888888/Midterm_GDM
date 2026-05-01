// using UnityEngine;

// #if UNITY_EDITOR
// using UnityEditor;
// #endif

// [RequireComponent(typeof(Canvas))]
// public class PauseManagerScript : MonoBehaviour
// {
//     [SerializeField] private Canvas canvas;

//     private PlayerCon m_Mobile;
//     private bool m_IsPlaying;

//     private void Awake()
//     {
//         m_IsPlaying = true;
//         m_Mobile = new PlayerCon();

//         if (canvas == null)
//             canvas = GetComponent<Canvas>();
//     }

//     private void Start()
//     {
//         if (canvas != null)
//             canvas.enabled = false;
//     }

//     private void OnEnable()
//     {
//         if (m_Mobile == null)
//             m_Mobile = new PlayerCon();

//         m_Mobile.Enable();
//         m_Mobile.Menu.Esc.performed += OnPausePerformed;
//     }

//     private void OnDisable()
//     {
//         if (m_Mobile != null)
//         {
//             m_Mobile.Menu.Esc.performed -= OnPausePerformed;
//             m_Mobile.Disable();
//         }
//     }

//     public void QuitGame()
//     {
// #if UNITY_EDITOR
//         EditorApplication.isPlaying = false;
// #else
//         Application.Quit();
// #endif
//     }

//     public void OnPausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
//     {
//         if (m_IsPlaying)
//             PauseGame();
//         else
//             ResumeGame();
//     }

//     public void ResumeGame()
//     {
//         Debug.Log("Resume Clicked");

//         m_IsPlaying = true;

//         AudioManager.I?.ResumeAllGameplayAudio();

//         Time.timeScale = 1f;
//         GameFlowController.I?.ResumeGame();

//         if (canvas != null)
//             canvas.enabled = false;
//     }

//     public void PauseGame()
//     {
//         m_IsPlaying = false;

//         GameFlowController.I?.PauseGame();
//         Time.timeScale = 0f;

//         AudioManager.I?.PauseAllGameplayAudio();

//         if (canvas != null)
//             canvas.enabled = true;
//     }
// }