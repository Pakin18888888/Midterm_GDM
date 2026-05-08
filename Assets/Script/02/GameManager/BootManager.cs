using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class BootManager : MonoBehaviour
{
    const string LOGOUT_KEY = "LOGOUT";
    async void Start()
    {
        await UGSInitializer.InitTask;

        // 🔥 Guest Login อัตโนมัติ
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance
                .SignInAnonymouslyAsync();
        }

        // 🔥 เช็คชื่อ
        if (string.IsNullOrEmpty(
            PlayerNameManager.Instance.GetName()))
        {
            SceneManager.LoadScene("NameScene");
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    public void Logout()
    {
        AuthenticationService.Instance
            .SignOut(true);

        PlayerPrefs.SetInt("LOGOUT", 1);

        PlayerNameManager.Instance
            .DeleteName();

        SceneManager.LoadScene("BootScene");
    }
}