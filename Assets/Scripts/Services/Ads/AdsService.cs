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
    [SerializeField] private int _matchesPerAdMin = 4;

    [SerializeField] private int _matchesPerAdMax = 5;

    private int _matchesSinceLastAd;
    private int _currentMatchesThreshold;

    private bool _rewardedReady;
    private bool _interstitialReady;

    private bool _initializing;

    private bool _rewardedLoading;
    private bool _interstitialLoading;

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

        ApplyOnlineState(InternetService.IsOnline, forceNotify: true);

        if (InternetService.IsOnline)
            TryInitializeOrLoad();
    }

    private void OnEnable()
    {
        InternetService.OnlineStateChanged += OnInternetChanged;
    }

    private void OnDisable()
    {
        InternetService.OnlineStateChanged -= OnInternetChanged;
    }

    public bool HasInternet() => InternetService.IsOnline;

    public RewardedState GetRewardedState() => _rewardedState;

    public bool CanShowRewarded()
        => _rewardedState == RewardedState.Ready
           && Advertisement.isInitialized
           && !Advertisement.isShowing
           && InternetService.IsOnline;

    public bool ShowRewarded(Action onReward)
    {
        if (onReward == null)
            return false;

        if (!CanShowRewarded())
            return false;

        _rewardCallback = onReward;
        SetRewardedState(RewardedState.Showing);

        Advertisement.Show(_rewardedPlacementId, this);
        return true;
    }

    public void NotifyMatchFinished()
    {
        _matchesSinceLastAd++;

        if (_matchesSinceLastAd < _currentMatchesThreshold)
            return;

        TryShowInterstitial();
    }

    private void OnInternetChanged(bool isOnline)
    {
        ApplyOnlineState(isOnline, forceNotify: false);

        if (!isOnline)
            return;

        TryInitializeOrLoad();
    }

    private void ApplyOnlineState(bool isOnline, bool forceNotify)
    {
        if (!isOnline)
        {
            _rewardedReady = false;
            _interstitialReady = false;

            _rewardedLoading = false;
            _interstitialLoading = false;

            _initializing = false;

            SetRewardedState(RewardedState.Offline, forceNotify);
        }
        else
        {
            if (forceNotify)
                RewardedStateChanged?.Invoke(_rewardedState);
        }
    }

    private void TryInitializeOrLoad()
    {
        if (!InternetService.IsOnline)
            return;

        if (!Advertisement.isInitialized)
        {
            if (_initializing)
                return;

            _initializing = true;
            SetRewardedState(RewardedState.Initializing);
            Advertisement.Initialize(_androidGameId, _testMode, this);
            return;
        }

        if (!_rewardedReady && !_rewardedLoading)
            LoadRewarded();

        if (!_interstitialReady && !_interstitialLoading)
            LoadInterstitial();

        if (!_rewardedReady && _rewardedState != RewardedState.Showing)
            SetRewardedState(RewardedState.Loading);
    }

    public void OnInitializationComplete()
    {
        _initializing = false;

        if (!InternetService.IsOnline)
        {
            SetRewardedState(RewardedState.Offline);
            return;
        }

        LoadRewarded();
        LoadInterstitial();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        _initializing = false;

        _rewardedReady = false;
        _interstitialReady = false;

        _rewardedLoading = false;
        _interstitialLoading = false;

        SetRewardedState(RewardedState.Failed);
    }

    private void LoadRewarded()
    {
        if (!InternetService.IsOnline)
        {
            SetRewardedState(RewardedState.Offline);
            return;
        }

        if (!Advertisement.isInitialized)
        {
            TryInitializeOrLoad();
            return;
        }

        _rewardedReady = false;

        if (_rewardedLoading)
            return;

        _rewardedLoading = true;
        SetRewardedState(RewardedState.Loading);

        Advertisement.Load(_rewardedPlacementId, this);
    }

    private void LoadInterstitial()
    {
        if (!InternetService.IsOnline)
            return;

        if (!Advertisement.isInitialized)
        {
            TryInitializeOrLoad();
            return;
        }

        _interstitialReady = false;

        if (_interstitialLoading)
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
            return;
        }

        if (placementId == _interstitialPlacementId)
        {
            _interstitialLoading = false;
            _interstitialReady = true;
            return;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (placementId == _rewardedPlacementId)
        {
            _rewardedLoading = false;
            _rewardedReady = false;

            SetRewardedState(InternetService.IsOnline ? RewardedState.Failed : RewardedState.Offline);
            return;
        }

        if (placementId == _interstitialPlacementId)
        {
            _interstitialLoading = false;
            _interstitialReady = false;
            return;
        }
    }

    public void OnUnityAdsShowStart(string placementId)
    { }

    public void OnUnityAdsShowClick(string placementId)
    { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state)
    {
        Time.timeScale = 1f;

        if (placementId == _rewardedPlacementId)
        {
            if (state == UnityAdsShowCompletionState.COMPLETED)
                _rewardCallback?.Invoke();

            _rewardCallback = null;

            _rewardedLoading = false;
            _rewardedReady = false;
            SetRewardedState(RewardedState.Loading);

            LoadRewarded();
        }

        if (placementId == _interstitialPlacementId)
        {
            ResetMatchCounter();
            LoadInterstitial();
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Time.timeScale = 1f;

        if (placementId == _rewardedPlacementId)
        {
            _rewardCallback = null;
            RewardedFailed?.Invoke();

            _rewardedLoading = false;
            _rewardedReady = false;
            SetRewardedState(RewardedState.Loading);

            LoadRewarded();
        }

        if (placementId == _interstitialPlacementId)
        {
            LoadInterstitial();
        }
    }

    private void TryShowInterstitial()
    {
        if (!InternetService.IsOnline)
            return;

        if (!Advertisement.isInitialized)
            return;

        if (Advertisement.isShowing)
            return;

        if (!_interstitialReady)
            return;

        Advertisement.Show(_interstitialPlacementId, this);
    }

    private void ResetMatchCounter()
    {
        _matchesSinceLastAd = 0;

        int min = Mathf.Min(_matchesPerAdMin, _matchesPerAdMax);
        int max = Mathf.Max(_matchesPerAdMin, _matchesPerAdMax);

        _currentMatchesThreshold = UnityEngine.Random.Range(min, max + 1);
    }

    private void SetRewardedState(RewardedState state, bool forceNotify = false)
    {
        if (!forceNotify && _rewardedState == state)
            return;

        _rewardedState = state;
        RewardedStateChanged?.Invoke(_rewardedState);
    }
}