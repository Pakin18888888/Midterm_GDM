using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    async void Start()
    {
        await UGSInitializer.InitTask;

        // if (AuthenticationService.Instance
        //     .IsSignedIn)
        // {
        //     Debug.Log("AUTO LOGIN");

        //     EnterGame();
        // }
    }

    public async void Login()
    {
        try
        {
            await AuthenticationService.Instance
                .SignInWithUsernamePasswordAsync(
                    emailInput.text,
                    passwordInput.text
                );

            Debug.Log("LOGIN SUCCESS");

            SceneManager.LoadScene("MainMenuScene");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public async void Register()
    {
        try
        {
            await AuthenticationService.Instance
                .SignUpWithUsernamePasswordAsync(
                    emailInput.text,
                    passwordInput.text
                );

            Debug.Log("REGISTER SUCCESS");

            SceneManager.LoadScene("MainMenuScene");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public async void GuestLogin()
    {
        try
        {
            await AuthenticationService.Instance
                .SignInAnonymouslyAsync();

            Debug.Log("GUEST LOGIN");

            SceneManager.LoadScene("MainMenuScene");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void EnterGame()
    {
        UnityEngine.SceneManagement.SceneManager
            .LoadScene("MainMenuScene");
    }
}