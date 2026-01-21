using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LobbyController : IDisposable
{
    public readonly struct AdsUiModel
    {
        public readonly bool Interactable;
        public readonly bool CanClick;
        public readonly float Alpha;
        public readonly bool ShowLoading;
        public readonly bool ShowConnecting;

        public AdsUiModel(bool interactable, bool canClick, float alpha, bool showLoading, bool showConnecting)
        {
            Interactable = interactable;
            CanClick = canClick;
            Alpha = alpha;
            ShowLoading = showLoading;
            ShowConnecting = showConnecting;
        }
    }

    private readonly LobbyService _service;
    private readonly SoundDefinition _emojiSelectSound;
    private readonly SoundDefinition _swapSound;
    private readonly GameRewardService _rewards;

    private bool _rewardedInProgress;
    private bool _wasOffline;

    private int _uiColorId = -1;
    private int _selectedEmojiId = -1;
    private int _selectedEmojiColorId = -1;

    private bool _disposed;

    public EmojiResolver Resolver { get; }

    public event Action<Sprite> PlayerAvatarChanged;

    public event Action<Sprite> AIAvatarChanged;

    public event Action<List<EmojiProgress>> EmojiListChanged;

    public event Action<string> PlayerNameChanged;

    public event Action<string> AINameChanged;

    public event Action AnyUserInteraction;

    public event Action<AdsUiModel> AdsUiChanged;

    public LobbyController(
        LobbyService service,
        EmojiResolver resolver,
        GameRewardService rewards,
        SoundDefinition emojiSelectSound,
        SoundDefinition colorChangeSound)
    {
        _service = service;
        Resolver = resolver;
        _rewards = rewards;
        _emojiSelectSound = emojiSelectSound;
        _swapSound = colorChangeSound;

        SettingsService.PlayerNameChanged += OnPlayerNameChanged;
    }

    public void Initialize()
    {
        if (AdsService.I != null)
            AdsService.I.RewardedStateChanged += OnRewardedStateChanged;

        InternetService.OnlineStateChanged += OnInternetStateChanged;

        var player = GameDataService.I.Data.Player;
        _selectedEmojiId = player.EmojiIndex;
        _selectedEmojiColorId = player.EmojiColor;

        PlayerNameChanged?.Invoke(player.Name);

        UpdateAdsUi();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        SettingsService.PlayerNameChanged -= OnPlayerNameChanged;
        InternetService.OnlineStateChanged -= OnInternetStateChanged;

        if (AdsService.I != null)
            AdsService.I.RewardedStateChanged -= OnRewardedStateChanged;

        _rewardedInProgress = false;
    }

    public void OnAdsPressed()
    {
        if (_disposed) return;

        AnyUserInteraction?.Invoke();

        if (_rewardedInProgress)
            return;

        if (!InternetService.IsOnline)
        {
            PopupService.I?.Show(PopupId.NoInternet);
            return;
        }

        if (AdsService.I == null)
            return;

        if (!AdsService.I.CanShowRewarded())
        {
            PopupService.I?.Show(PopupId.NoInternet);
            return;
        }

        _rewardedInProgress = true;

        if (!AdsService.I.ShowRewarded(OnRewarded))
            _rewardedInProgress = false;

        UpdateAdsUi();
    }

    private void OnRewarded()
    {
        if (_disposed)
            return;

        _rewardedInProgress = false;

        bool hasInternet = InternetService.IsOnline;
        var result = _rewards.RewardedOpened(hasInternet);

        UpdateEmojiList();

        PopupService.I?.Show(GetRewardPopup(result));

        UpdateAdsUi();
    }

    private PopupId GetRewardPopup(GameRewardResult result)
    {
        if (result.EmojiUnlocked)
            return PopupId.Reward;

        return result.BlockReason switch
        {
            RewardBlockReason.NoInternet => PopupId.NoInternet,
            RewardBlockReason.AllUnlocked => PopupId.Complete,
            _ => PopupId.Complete
        };
    }

    private void OnRewardedStateChanged(AdsService.RewardedState state)
    {
        if (_disposed) return;
        UpdateAdsUi();
    }

    private void OnInternetStateChanged(bool isOnline)
    {
        if (_disposed) return;

        if (!isOnline)
            _wasOffline = true;

        UpdateAdsUi();
    }

    private void UpdateAdsUi()
    {
        if (_disposed) return;

        if (AdsService.I == null)
        {
            AdsUiChanged?.Invoke(new AdsUiModel(
                interactable: false,
                canClick: false,
                alpha: 0.3f,
                showLoading: false,
                showConnecting: false));
            return;
        }

        bool online = InternetService.IsOnline;
        var state = AdsService.I.GetRewardedState();

        if (!online)
        {
            AdsUiChanged?.Invoke(new AdsUiModel(
                interactable: true,
                canClick: true,
                alpha: 0.3f,
                showLoading: false,
                showConnecting: false));
            return;
        }

        if (_wasOffline && state != AdsService.RewardedState.Ready)
        {
            AdsUiChanged?.Invoke(new AdsUiModel(
                interactable: false,
                canClick: false,
                alpha: 0.1f,
                showLoading: false,
                showConnecting: true));
            return;
        }

        _wasOffline = false;

        if (state == AdsService.RewardedState.Ready && !_rewardedInProgress)
        {
            AdsUiChanged?.Invoke(new AdsUiModel(
                interactable: true,
                canClick: true,
                alpha: 1f,
                showLoading: false,
                showConnecting: false));
            return;
        }

        AdsUiChanged?.Invoke(new AdsUiModel(
            interactable: false,
            canClick: false,
            alpha: 0.1f,
            showLoading: true,
            showConnecting: false));
    }

    public void SetInitialColor(int colorId)
    {
        if (_disposed) return;

        _uiColorId = colorId;
        UpdateEmojiList();

        var ai = _service.EnsureValidAIEmoji();
        if (ai != null)
            AIAvatarChanged?.Invoke(ai);

        AINameChanged?.Invoke(
            string.IsNullOrEmpty(_service.GetAIName())
                ? _service.GenerateAIName()
                : _service.GetAIName());
    }

    public void OnColorChanged(int colorId)
    {
        if (_disposed) return;

        AnyUserInteraction?.Invoke();

        if (_uiColorId == colorId)
            return;

        _uiColorId = colorId;
        UpdateEmojiList();
        PlayColorChangeSound();
    }

    public void OnEmojiSelected(int emojiId)
    {
        if (_disposed) return;

        AnyUserInteraction?.Invoke();

        if (_selectedEmojiId == emojiId &&
            _selectedEmojiColorId == _uiColorId)
            return;

        _selectedEmojiId = emojiId;
        _selectedEmojiColorId = _uiColorId;

        var player = _service.SelectPlayerEmoji(_uiColorId, emojiId);
        if (player != null)
        {
            PlayerAvatarChanged?.Invoke(player);
            PlayEmojiSelectSound();
        }

        var ai = _service.EnsureValidAIEmoji();
        if (ai != null)
            AIAvatarChanged?.Invoke(ai);
    }

    public void OnAIStrategyChanged(AIStrategyType type)
    {
        if (_disposed) return;

        var data = GameDataService.I.Data;
        data.AI.Strategy = type;
        GameDataService.I.Save();

        AINameChanged?.Invoke(_service.GenerateAIName());
    }

    public void OnStartPressed()
    {
        if (_disposed) return;

        PlayColorChangeSound();
        SceneManager.LoadScene("Main");
    }

    private void UpdateEmojiList()
    {
        if (_disposed) return;

        if (_uiColorId < 0)
            return;

        var progress = GameDataService.I.Data.Progress;
        var data = Resolver.GetData(_uiColorId);

        if (data == null)
            return;

        EmojiListChanged?.Invoke(
            progress.GetSortedForView(_uiColorId, data.EmojiSprites.Count));
    }

    private void PlayEmojiSelectSound()
    {
        if (_disposed) return;

        if (_emojiSelectSound != null && AudioService.I != null)
            AudioService.I.PlaySFX(_emojiSelectSound);
    }

    private void PlayColorChangeSound()
    {
        if (_disposed) return;

        if (_swapSound != null && AudioService.I != null)
            AudioService.I.PlaySFX(_swapSound);
    }

    private void OnPlayerNameChanged(string name)
    {
        if (_disposed) return;
        PlayerNameChanged?.Invoke(name);
    }
}