using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsPopup : PopupBase
{
    public override PopupId Id => PopupId.Settings;

    [Header("Music")]
    [SerializeField] private Button _musicButton;

    [SerializeField] private Image _musicOffIcon;
    [SerializeField] private Slider _musicSlider;

    [Header("SFX")]
    [SerializeField] private Button _sfxButton;

    [SerializeField] private Image _sfxOffIcon;
    [SerializeField] private Slider _sfxSlider;

    [Header("Vibration")]
    [SerializeField] private Button _vibrationButton;

    [SerializeField] private Image _vibrationOffIcon;

    [Header("Player")]
    [SerializeField] private TMP_InputField _playerNameInput;

    [Header("Buttons")]
    [SerializeField] private Button _closeButton;

    [SerializeField] private Button _aboutInfoButton;

    private SettingsService _settings;
    private bool _bound;

    protected override void Awake()
    {
        base.Awake();
        _settings = SettingsService.I;
        _closeButton.onClick.AddListener(OnCloseClicked);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _closeButton.onClick.RemoveListener(OnCloseClicked);
    }

    public override void Show()
    {
        base.Show();
        Refresh();
        Bind();
    }

    public override void Hide()
    {
        Unbind();
        base.Hide();
    }

    private void Bind()
    {
        if (_bound)
            return;

        _bound = true;

        _musicButton.onClick.AddListener(OnMusicToggle);
        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        _sfxButton.onClick.AddListener(OnSfxToggle);
        _sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

        _vibrationButton.onClick.AddListener(OnVibrationToggle);
        _playerNameInput.onEndEdit.AddListener(_settings.SetPlayerName);

        _aboutInfoButton.onClick.AddListener(OnAboutCkliked);
    }

    private void Unbind()
    {
        if (!_bound)
            return;

        _bound = false;

        _musicButton.onClick.RemoveListener(OnMusicToggle);
        _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);

        _sfxButton.onClick.RemoveListener(OnSfxToggle);
        _sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);

        _vibrationButton.onClick.RemoveListener(OnVibrationToggle);
        _playerNameInput.onEndEdit.RemoveListener(_settings.SetPlayerName);

        _aboutInfoButton.onClick.RemoveListener(OnAboutCkliked);
    }

    private void OnAboutCkliked()
    {
        PopupService.I.Show(PopupId.About);
    }

    private void OnMusicToggle()
    {
        _settings.SetMusicEnabled(!_settings.Data.MusicEnabled);
        AudioService.I.RefreshMusicVolume();
        RefreshVisuals();
    }

    private void OnMusicVolumeChanged(float value)
    {
        _settings.SetMusicVolume(value);
        AudioService.I.RefreshMusicVolume();
    }

    private void OnSfxToggle()
    {
        _settings.SetSfxEnabled(!_settings.Data.SfxEnabled);
        RefreshVisuals();
    }

    private void OnSfxVolumeChanged(float value)
    {
        _settings.SetSfxVolume(value);
    }

    private void OnVibrationToggle()
    {
        bool newValue = !_settings.Data.VibrationEnabled;
        _settings.SetVibration(newValue);

        if (newValue && VibrationService.I != null)
        {
            VibrationService.I.Light();
        }

        RefreshVisuals();
    }

    private void OnCloseClicked()
    {
        PopupService.I.HideCurrent();
    }

    private void Refresh()
    {
        var data = _settings.Data;
        var player = GameDataService.I.Data.Player;

        _musicSlider.SetValueWithoutNotify(data.MusicVolume);
        _sfxSlider.SetValueWithoutNotify(data.SfxVolume);
        _playerNameInput.SetTextWithoutNotify(player.Name);

        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        var data = _settings.Data;

        _musicOffIcon.enabled = !data.MusicEnabled;
        _musicSlider.interactable = data.MusicEnabled;

        _sfxOffIcon.enabled = !data.SfxEnabled;
        _sfxSlider.interactable = data.SfxEnabled;

        _vibrationOffIcon.enabled = !data.VibrationEnabled;
    }
}