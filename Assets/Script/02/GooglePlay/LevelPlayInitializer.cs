using UnityEngine;
using Unity.Services.LevelPlay;

public class LevelPlayInitializer : MonoBehaviour
{
    [SerializeField] private string appKey = "25a05013d";
    [SerializeField] private InterstitialAdController interstitialAdController;

    public static bool IsInitialized { get; private set; }

    private void Start()
    {
        Debug.Log("[LevelPlay] Initializing SDK...");

        LevelPlay.OnInitSuccess += OnInitSuccess;
        LevelPlay.OnInitFailed += OnInitFailed;

        LevelPlay.SetMetaData("is_test_suite", "enable");

        LevelPlay.Init(appKey);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void OnInitSuccess(LevelPlayConfiguration configuration)
    {
        IsInitialized = true;

        Debug.Log("[LevelPlay] SDK initialized successfully.");

        LevelPlay.LaunchTestSuite();

        if (interstitialAdController != null)
        {
            interstitialAdController.InitializeInterstitial();
            interstitialAdController.LoadInterstitial();
        }
    }

    private void OnInitFailed(LevelPlayInitError error)
    {
        IsInitialized = false;
        Debug.LogError("[LevelPlay] SDK initialization failed: " + error);
    }

    private void OnDestroy()
    {
        LevelPlay.OnInitSuccess -= OnInitSuccess;
        LevelPlay.OnInitFailed -= OnInitFailed;
    }
}
