using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class UGSInitializer : MonoBehaviour
{
    public static Task InitTask => InitializeOnce();

    static Task initTask;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    static async Task InitializeOnce()
    {
        if (initTask != null)
        {
            await initTask;
            return;
        }

        initTask = Initialize();

        await initTask;
    }

    static async Task Initialize()
    {
        await UnityServices.InitializeAsync();

        // 🔥 auto guest login
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance
                .SignInAnonymouslyAsync();
        }

        Debug.Log("UGS Login Success");
    }
}