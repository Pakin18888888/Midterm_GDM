using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.AudioSettings;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Canvas))]
public class PauseManagerScript : MonoBehaviour
{
    public Canvas canvas;
    private PlayerCon m_Mobile;
    private bool m_IsPlaying;
    
    private void OnEnable()
    {
        m_Mobile.Enable();
        m_Mobile.Menu.Esc.performed += OnPausePerformed;

    }
    private void OnDisable()
    {
        m_Mobile.Disable();
        m_Mobile.Menu.Esc.performed -= OnPausePerformed;
    }

    void Start()
    {
        canvas.enabled = false;
    }
    void Awake()
    {
        m_IsPlaying = true;
        m_Mobile = new PlayerCon();
        //canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     canvas.enabled = !canvas.enabled;
        //     OnPausePerformed();
        // }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    
    public void OnPausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (m_IsPlaying)
            PauseGame();
        else
            ResumeGame();
    }
    public void ResumeGame()
    {
        Debug.Log("Resume Clicked");

        m_IsPlaying = true;
        GameManager.I?.SetPlayerControl(true);
        Time.timeScale = 1;
        canvas.enabled = false;
    }

    public void PauseGame()
    {
        m_IsPlaying = false;

        GameManager.I?.SetPlayerControl(false);

        Time.timeScale = 0;

        canvas.enabled = true;
    }
}