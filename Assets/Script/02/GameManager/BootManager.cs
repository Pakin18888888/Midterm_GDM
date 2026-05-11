using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class BootManager : MonoBehaviour
{
    async void Start()
    {
        await UGSInitializer.InitTask;

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance
                .SignInAnonymouslyAsync();
        }

        // SceneManager.LoadScene("START");
    }
}