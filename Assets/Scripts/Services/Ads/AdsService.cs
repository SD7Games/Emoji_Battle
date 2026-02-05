using System;
using UnityEngine;
using UnityEngine.Advertisements;

public sealed class AdsService :
    MonoBehaviour,
    IUnityAdsInitializationListener,
    IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    public static AdsService I { get; private set; }

    public enum RewardedState
    {
        Offline,
        Initializing,
        Loading,
        Ready,
        Showing,
        Failed
    }

    [Header("Unity Ads")]
    [SerializeField] private string _androidGameId = "6020853";

    [SerializeField] private string _rewardedPlacementId = "Rewarded_Android";
    [SerializeField] private string _interstitialPlacementId = "Interstitial_Android";
    [SerializeField] private bool _testMode = false;

    [Header("Auto Ads")]
    [SerializeField] private int _matchesPerAdMin = 3;

    [SerializeField] private int _matchesPerAdMax = 4;

    private int _matchesSinceLastAd;
    private int _currentMatchesThreshold;

    private bool _rewardedReady;
    private bool _interstitialReady;

    private bool _rewardedLoading;
    private bool _interstitialLoading;

    private bool _initializing;
    private int _lastResumeFrame = -1;

    private Action _rewardCallback;
    private RewardedState _rewardedState = RewardedState.Offline;

    public event Action RewardedFailed;

    public event Action RewardedReady;

    public event Action<RewardedState> RewardedStateChanged;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

        ResetMatchCounter();

        InternetService.OnlineStateChanged += OnInternetChanged;

        if (InternetService.IsOnline)
            InitializeAds();
        else
            SetRewardedState(RewardedState.Offline, true);
    }

    private void OnDestroy()
    {
        InternetService.OnlineStateChanged -= OnInternetChanged;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            OnResume();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            OnResume();
    }

    public RewardedState GetRewardedState() => _rewardedState;

    public bool CanShowRewarded()
    {
        return _rewardedReady &&
               Advertisement.isInitialized &&
               !Advertisement.isShowing &&
               InternetService.IsOnline;
    }

    public bool ShowRewarded(Action onReward)
    {
        if (onReward == null || !CanShowRewarded())
            return false;

        _rewardCallback = onReward;
        SetRewardedState(RewardedState.Showing);

        Advertisement.Show(_rewardedPlacementId, this);
        return true;
    }

    public void NotifyMatchFinished()
    {
        _matchesSinceLastAd++;

        if (_matchesSinceLastAd >= _currentMatchesThreshold)
        {
            if (TryShowInterstitial())
                ResetMatchCounter();
        }
    }

    private void OnResume()
    {
        if (_lastResumeFrame == Time.frameCount)
            return;

        _lastResumeFrame = Time.frameCount;

        if (!InternetService.IsOnline || Advertisement.isShowing)
            return;

        _rewardedLoading = false;
        _interstitialLoading = false;

        if (!Advertisement.isInitialized)
            InitializeAds();
        else
        {
            RequestRewarded();
            RequestInterstitial();
        }
    }

    private void OnInternetChanged(bool online)
    {
        if (!online)
        {
            _initializing = false;

            _rewardedReady = false;
            _interstitialReady = false;

            _rewardedLoading = false;
            _interstitialLoading = false;

            _lastResumeFrame = -1;

            SetRewardedState(RewardedState.Offline, true);
            return;
        }

        OnResume();
    }

    private void InitializeAds()
    {
        if (_initializing)
            return;

        _initializing = true;
        SetRewardedState(RewardedState.Initializing);

        Advertisement.Initialize(_androidGameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        _initializing = false;

        if (!InternetService.IsOnline)
        {
            SetRewardedState(RewardedState.Offline);
            return;
        }

        RequestRewarded();
        RequestInterstitial();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        _initializing = false;
        _rewardedLoading = false;
        _interstitialLoading = false;

        SetRewardedState(RewardedState.Failed);
    }

    private void RequestRewarded()
    {
        if (_rewardedLoading || _rewardedReady)
            return;

        _rewardedLoading = true;
        SetRewardedState(RewardedState.Loading);

        Advertisement.Load(_rewardedPlacementId, this);
    }

    private void RequestInterstitial()
    {
        if (_interstitialLoading || _interstitialReady)
            return;

        _interstitialLoading = true;
        Advertisement.Load(_interstitialPlacementId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == _rewardedPlacementId)
        {
            _rewardedLoading = false;
            _rewardedReady = true;
            SetRewardedState(RewardedState.Ready);
            RewardedReady?.Invoke();
        }
        else if (placementId == _interstitialPlacementId)
        {
            _interstitialLoading = false;
            _interstitialReady = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (placementId == _rewardedPlacementId)
        {
            _rewardedLoading = false;
            _rewardedReady = false;
            SetRewardedState(RewardedState.Failed);
        }
        else if (placementId == _interstitialPlacementId)
        {
            _interstitialLoading = false;
            _interstitialReady = false;
        }
    }

    private bool TryShowInterstitial()
    {
        if (!InternetService.IsOnline ||
            !_interstitialReady ||
            !Advertisement.isInitialized ||
            Advertisement.isShowing)
            return false;

        Advertisement.Show(_interstitialPlacementId, this);
        return true;
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        if (placementId == _interstitialPlacementId)
            ResetMatchCounter();
    }

    public void OnUnityAdsShowClick(string placementId)
    { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state)
    {
        if (placementId == _rewardedPlacementId)
        {
            if (state == UnityAdsShowCompletionState.COMPLETED)
                _rewardCallback?.Invoke();

            _rewardCallback = null;
            _rewardedReady = false;
            RequestRewarded();
        }
        else if (placementId == _interstitialPlacementId)
        {
            _interstitialReady = false;
            RequestInterstitial();
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        if (placementId == _rewardedPlacementId)
        {
            _rewardCallback = null;
            RewardedFailed?.Invoke();
            _rewardedReady = false;
            RequestRewarded();
        }
        else if (placementId == _interstitialPlacementId)
        {
            _interstitialReady = false;
            RequestInterstitial();
        }
    }

    private void ResetMatchCounter()
    {
        _matchesSinceLastAd = 0;
        _currentMatchesThreshold =
            UnityEngine.Random.Range(_matchesPerAdMin, _matchesPerAdMax + 1);
    }

    private void SetRewardedState(RewardedState state, bool force = false)
    {
        if (!force && _rewardedState == state)
            return;

        _rewardedState = state;
        RewardedStateChanged?.Invoke(_rewardedState);
    }
}