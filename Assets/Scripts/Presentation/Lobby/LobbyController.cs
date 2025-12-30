using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LobbyController : IDisposable
{
    private readonly LobbyService _service;
    private readonly SoundDefinition _emojiSelectSound;
    private readonly SoundDefinition _colorChangeSound;

    private int _currentColor;
    private int _currentEmojiId = -1;

    public EmojiResolver Resolver { get; }

    public event Action<Sprite> PlayerAvatarChanged;

    public event Action<Sprite> AIAvatarChanged;

    public event Action<List<EmojiProgress>> EmojiListChanged;

    public event Action<string> PlayerNameChanged;

    public event Action<string> AINameChanged;

    public LobbyController(
        LobbyService service,
        EmojiResolver resolver,
        SoundDefinition emojiChooseSound,
        SoundDefinition colorChangeSound
    )
    {
        _service = service;
        Resolver = resolver;
        _emojiSelectSound = emojiChooseSound;
        _colorChangeSound = colorChangeSound;

        SettingsService.PlayerNameChanged += OnPlayerNameChanged;
    }

    public void Initialize()
    {
        var player = GameDataService.I.Data.Player;
        _currentEmojiId = player.EmojiIndex;

        PlayerNameChanged?.Invoke(player.Name);
    }

    public void Dispose()
    {
        SettingsService.PlayerNameChanged -= OnPlayerNameChanged;
    }

    public void OnAIStrategyChanged(AIStrategyType type)
    {
        var data = GameDataService.I.Data;
        data.AI.Strategy = type;
        GameDataService.I.Save();

        AINameChanged?.Invoke(_service.GenerateAIName());
    }

    public void SetInitialColor(int colorId)
    {
        _currentColor = colorId;
        UpdateEmojiList();

        var ai = _service.EnsureValidAIEmoji();
        if (ai != null)
            AIAvatarChanged?.Invoke(ai);

        AINameChanged?.Invoke(
            string.IsNullOrEmpty(_service.GetAIName())
                ? _service.GenerateAIName()
                : _service.GetAIName()
        );
    }

    public void OnColorChanged(int colorId)
    {
        if (_currentColor == colorId)
            return;

        _currentColor = colorId;
        UpdateEmojiList();
        PlayColorChangeSound();
    }

    public void OnEmojiSelected(int emojiId)
    {
        if (_currentEmojiId == emojiId)
            return;

        _currentEmojiId = emojiId;

        var player = _service.SelectPlayerEmoji(_currentColor, emojiId);
        if (player != null)
        {
            PlayerAvatarChanged?.Invoke(player);
            PlayEmojiSelectSound();
        }

        var ai = _service.EnsureValidAIEmoji();
        if (ai != null)
            AIAvatarChanged?.Invoke(ai);
    }

    private void PlayEmojiSelectSound()
    {
        if (_emojiSelectSound == null)
            return;

        AudioService.I.Play(_emojiSelectSound);
    }

    private void PlayColorChangeSound()
    {
        if (_colorChangeSound == null)
            return;

        AudioService.I.Play(_colorChangeSound);
    }

    public void OnStartPressed()
    {
        SceneManager.LoadScene("Main");
    }

    private void UpdateEmojiList()
    {
        var progress = GameDataService.I.Data.Progress;
        var data = Resolver.GetData(_currentColor);

        if (data == null)
            return;

        EmojiListChanged?.Invoke(
            progress.GetSortedForView(
                _currentColor,
                data.EmojiSprites.Count
            )
        );
    }

    private void OnPlayerNameChanged(string name)
    {
        PlayerNameChanged?.Invoke(name);
    }
}