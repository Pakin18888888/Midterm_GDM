using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class BootManager : MonoBehaviour
{
    async void Start()
    {
        await UGSInitializer.InitTask;
        SceneManager.LoadScene("LoginScene");

        // if (AuthenticationService.Instance.SessionTokenExists)
        // {
        //     // Debug.Log("AUTO LOGIN");

        //     // SceneManager.LoadScene("MainMenuScene");
        // }
        // else
        // {
        //     Debug.Log("GO LOGIN");

        //     SceneManager.LoadScene("LoginScene");
        // }
    }
}