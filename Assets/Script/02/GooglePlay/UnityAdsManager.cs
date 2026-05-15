using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour,
    IUnityAdsInitializationListener,
    IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    [SerializeField] private string androidGameId = "6115858";
    [SerializeField] private string interstitialAdId = "Interstitial_Android";
    [SerializeField] private bool testMode = true;
    private Action onAdClosed;
    public static UnityAdsManager Instance;


    private bool adLoaded = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Advertisement.Initialize(androidGameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialized");

        Advertisement.Load(interstitialAdId, this);
    }
    public void ShowAd(Action callback = null)
    {
        onAdClosed = callback;

        Debug.Log("Trying to show ad...");
        Debug.Log("Ad Loaded State: " + adLoaded);

        if (adLoaded)
        {
            Advertisement.Show(interstitialAdId, this);
        }
        else
        {
            Debug.Log("Ad not loaded yet");

            callback?.Invoke();
        }
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded");

        adLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId,
        UnityAdsLoadError error,
        string message)
    {
        Debug.LogError($"Load Failed: {error} - {message}");

        Invoke(nameof(RetryLoadAd), 5f);
    }

    void RetryLoadAd()
    {
        Debug.Log("Retrying Ad Load...");

        Advertisement.Load(interstitialAdId, this);
    }

    public void OnUnityAdsShowFailure(string adUnitId,
        UnityAdsShowError error,
        string message)
    {
        Debug.LogError($"Show Failed: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log("Ad Started");
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log("Ad Clicked");
    }

    public void OnUnityAdsShowComplete(
    string adUnitId,
    UnityAdsShowCompletionState state)
    {
        Debug.Log("Ad Closed");

        adLoaded = false;

        Advertisement.Load(interstitialAdId, this);

        onAdClosed?.Invoke();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error,
        string message)
    {
        Debug.LogError($"Init Failed: {error} - {message}");
    }
}