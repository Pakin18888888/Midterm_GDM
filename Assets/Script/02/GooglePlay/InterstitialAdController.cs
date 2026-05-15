// using UnityEngine;
// using Unity.Services.LevelPlay;
// using System;

// public class InterstitialAdController : MonoBehaviour
// {
//     public static InterstitialAdController Instance;

//     void Awake()
//     {
//         if (Instance != null)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;

//         DontDestroyOnLoad(gameObject);
//     }
//     [SerializeField] private string interstitialAdUnitId = "smjp2fyczvdprtbi";

//     private LevelPlayInterstitialAd interstitialAd;
//     private bool isReady;

//     private Action onAdClosedCallback;

//     public void InitializeInterstitial()
//     {
//         interstitialAd = new LevelPlayInterstitialAd(interstitialAdUnitId);

//         interstitialAd.OnAdLoaded += OnAdLoaded;
//         interstitialAd.OnAdLoadFailed += OnAdLoadFailed;
//         interstitialAd.OnAdDisplayed += OnAdDisplayed;
//         interstitialAd.OnAdDisplayFailed += OnAdDisplayFailed;
//         interstitialAd.OnAdClicked += OnAdClicked;
//         interstitialAd.OnAdClosed += OnAdClosed;
//     }

//     public void LoadInterstitial()
//     {
//         isReady = false;
//         interstitialAd.LoadAd();
//     }

//     public void ShowInterstitial(Action callback)
//     {
//         // เก็บ callback ไว้
//         onAdClosedCallback = callback;

//         if (!isReady)
//         {
//             Debug.LogWarning("[Interstitial] Ad not ready.");

//             // ถ้าโฆษณาไม่พร้อม ให้ทำต่อทันที
//             callback?.Invoke();
//             return;
//         }

//         interstitialAd.ShowAd();
//     }

//     private void OnAdLoaded(LevelPlayAdInfo adInfo)
//     {
//         isReady = true;
//         Debug.Log("[Interstitial] Ad loaded: " + adInfo);
//     }

//     private void OnAdLoadFailed(LevelPlayAdError error)
//     {
//         isReady = false;
//         Debug.LogError("[Interstitial] Load failed: " + error);
//     }

//     private void OnAdDisplayed(LevelPlayAdInfo adInfo)
//     {
//         Debug.Log("[Interstitial] Ad displayed: " + adInfo);
//     }

//     private void OnAdDisplayFailed(LevelPlayAdInfo adInfo, LevelPlayAdError error)
//     {
//         Debug.LogError("[Interstitial] Display failed: " + error);

//         // ถ้าแสดงไม่ได้ ก็ทำ callback ต่อ
//         onAdClosedCallback?.Invoke();
//     }

//     private void OnAdClicked(LevelPlayAdInfo adInfo)
//     {
//         Debug.Log("[Interstitial] Ad clicked: " + adInfo);
//     }

//     private void OnAdClosed(LevelPlayAdInfo adInfo)
//     {
//         Debug.Log("[Interstitial] Ad closed: " + adInfo);

//         isReady = false;

//         // โหลดรอรอบถัดไป
//         LoadInterstitial();

//         // ทำงานต่อหลังปิดโฆษณา
//         onAdClosedCallback?.Invoke();
//     }
// }