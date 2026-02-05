using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LobbyView : MonoBehaviour
{
    [Header("Emoji")]
    [SerializeField] private EmojiScrollView _emojiView;

    [SerializeField] private ColorSwitcherView _colorSwitcher;

    [Header("Avatars")]
    [SerializeField] private DissolveLobby _playerDissolve;

    [SerializeField] private DissolveLobby _aiDissolve;

    [Header("Text")]
    [SerializeField] private TMP_Text _playerNameText;

    [SerializeField] private TMP_Text _aiNameText;

    [Header("Buttons")]
    [SerializeField] private Button _startButton;

    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _adsButton;
    [SerializeField] private Button _complexityInfoButton;

    [Header("Ads UI")]
    [SerializeField] private GameObject _adsLoadingText;

    [SerializeField] private GameObject _adsConnectingText;

    private LobbyController _controller;

    private Sprite _lastPlayerSprite;
    private Sprite _lastAISprite;

    private Graphic[] _adsButtonGraphics;

    public void Construct(LobbyController controller)
    {
        _controller = controller;

        if (_adsButton != null)
            _adsButtonGraphics = _adsButton.GetComponentsInChildren<Graphic>(true);

        if (_emojiView != null)
            _emojiView.OnEmojiClicked += _controller.OnEmojiSelected;

        if (_colorSwitcher != null)
            _colorSwitcher.OnColorSelected += _controller.OnColorChanged;

        if (_startButton != null)
            _startButton.onClick.AddListener(_controller.OnStartPressed);

        if (_settingsButton != null)
            _settingsButton.onClick.AddListener(OnSettingsPressed);

        if (_adsButton != null)
            _adsButton.onClick.AddListener(_controller.OnAdsPressed);

        if (_complexityInfoButton != null)
            _complexityInfoButton.onClick.AddListener(OnComplexityInfoPressed);

        _controller.PlayerAvatarChanged += UpdatePlayerAvatar;
        _controller.AIAvatarChanged += UpdateAIAvatar;
        _controller.EmojiListChanged += UpdateEmojiList;
        _controller.PlayerNameChanged += UpdatePlayerName;
        _controller.AINameChanged += UpdateAIName;
        _controller.AdsUiChanged += ApplyAdsUi;
    }

    private void OnDestroy()
    {
        if (_controller == null)
            return;

        if (_emojiView != null)
            _emojiView.OnEmojiClicked -= _controller.OnEmojiSelected;

        if (_colorSwitcher != null)
            _colorSwitcher.OnColorSelected -= _controller.OnColorChanged;

        if (_startButton != null)
            _startButton.onClick.RemoveListener(_controller.OnStartPressed);

        if (_settingsButton != null)
            _settingsButton.onClick.RemoveListener(OnSettingsPressed);

        if (_adsButton != null)
            _adsButton.onClick.RemoveListener(_controller.OnAdsPressed);

        if (_complexityInfoButton != null)
            _complexityInfoButton.onClick.RemoveListener(OnComplexityInfoPressed);

        _controller.PlayerAvatarChanged -= UpdatePlayerAvatar;
        _controller.AIAvatarChanged -= UpdateAIAvatar;
        _controller.EmojiListChanged -= UpdateEmojiList;
        _controller.PlayerNameChanged -= UpdatePlayerName;
        _controller.AINameChanged -= UpdateAIName;
        _controller.AdsUiChanged -= ApplyAdsUi;
    }

    public void ForceSetPlayerAvatar(Sprite sprite)
    {
        _lastPlayerSprite = sprite;
        _playerDissolve.SetSprite(sprite);
        _playerDissolve.PlayForOwner(AvatarOwner.Player);
    }

    private void ApplyAdsUi(LobbyController.AdsUiModel model)
    {
        if (_adsButton != null)
            _adsButton.interactable = model.Interactable;

        SetAdsButtonAlpha(model.Alpha);

        if (_adsLoadingText != null)
            _adsLoadingText.SetActive(model.ShowLoading);

        if (_adsConnectingText != null)
            _adsConnectingText.SetActive(model.ShowConnecting);
    }

    private void SetAdsButtonAlpha(float alpha)
    {
        if (_adsButtonGraphics == null || _adsButtonGraphics.Length == 0)
            return;

        alpha = Mathf.Clamp01(alpha);

        for (int i = 0; i < _adsButtonGraphics.Length; i++)
        {
            var g = _adsButtonGraphics[i];
            if (g == null) continue;

            Color c = g.color;
            c.a = alpha;
            g.color = c;
        }
    }

    private void OnSettingsPressed()
    {
        PopupService.I.Show(PopupId.Settings);
    }

    private void OnComplexityInfoPressed()
    {
        PopupService.I.Show(PopupId.ComplexityInfo);
    }

    private void UpdatePlayerName(string name)
    {
        if (_playerNameText != null)
            _playerNameText.text = name;
    }

    private void UpdateAIName(string name)
    {
        if (_aiNameText != null)
            _aiNameText.text = name;
    }

    private void UpdateEmojiList(List<EmojiProgress> items)
    {
        if (_emojiView != null)
            _emojiView.Fill(items, _controller.Resolver);
    }

    private void UpdatePlayerAvatar(Sprite sprite)
    {
        if (sprite == null)
            return;

        bool changed = _lastPlayerSprite != sprite;
        _lastPlayerSprite = sprite;

        _playerDissolve.SetSprite(sprite);

        if (changed)
            _playerDissolve.PlayForOwner(AvatarOwner.Player);
    }

    private void UpdateAIAvatar(Sprite sprite)
    {
        if (sprite == null)
            return;

        bool changed = _lastAISprite != sprite;
        _lastAISprite = sprite;

        _aiDissolve.SetSprite(sprite);

        if (changed)
            _aiDissolve.PlayForOwner(AvatarOwner.AI);
    }
}