using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LobbyController : IDisposable
{
    private readonly LobbyService _service;
    private int _currentColor;

    public EmojiResolver Resolver { get; }

    public event Action<Sprite> PlayerAvatarChanged;

    public event Action<Sprite> AIAvatarChanged;

    public event Action<List<EmojiProgress>> EmojiListChanged;

    public event Action<string> PlayerNameChanged;

    public event Action<string> AINameChanged;

    public LobbyController(
        LobbyService service,
        EmojiResolver resolver
    )
    {
        _service = service;
        Resolver = resolver;

        SettingsService.PlayerNameChanged += OnPlayerNameChanged;
    }

    public void Initialize()
    {
        PlayerNameChanged?.Invoke(GetPlayerName());
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

        string newName = _service.GenerateAIName();
        AINameChanged?.Invoke(newName);
    }

    public void SetInitialColor(int colorId)
    {
        _currentColor = colorId;
        UpdateEmojiList();

        var ai = _service.EnsureValidAIEmoji();
        if (ai != null)
            AIAvatarChanged?.Invoke(ai);

        string aiName = _service.GetAIName();
        if (string.IsNullOrEmpty(aiName))
            aiName = _service.GenerateAIName();

        AINameChanged?.Invoke(aiName);
    }

    public void OnColorChanged(int colorId)
    {
        _currentColor = colorId;
        UpdateEmojiList();
    }

    public void OnEmojiSelected(int emojiId)
    {
        var player = _service.SelectPlayerEmoji(_currentColor, emojiId);
        if (player != null)
            PlayerAvatarChanged?.Invoke(player);

        var ai = _service.EnsureValidAIEmoji();
        if (ai != null)
            AIAvatarChanged?.Invoke(ai);
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

        var list = progress.GetSortedForView(
            _currentColor,
            data.EmojiSprites.Count
        );

        EmojiListChanged?.Invoke(list);
    }

    private void OnPlayerNameChanged(string name)
    {
        PlayerNameChanged?.Invoke(name);
    }

    private string GetPlayerName()
    {
        return GameDataService.I.Data.Player.Name;
    }
}