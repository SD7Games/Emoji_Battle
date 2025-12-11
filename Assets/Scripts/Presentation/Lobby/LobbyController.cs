using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LobbyController
{
    private readonly LobbyService _service;
    private int _currentColor;

    public event Action<Sprite> AIAvatarChanged;

    public event Action<Sprite> PlayerAvatarChanged;

    public event Action<EmojiViewData[]> EmojiListChanged;

    public event Action<string> AINameChanged;

    public LobbyController(LobbyService service)
    {
        _service = service;
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

        Sprite ai = _service.EnsureValidAIEmoji();
        if (ai != null)
            AIAvatarChanged?.Invoke(ai);

        string name = _service.GetAIName();
        if (string.IsNullOrEmpty(name))
            name = _service.GenerateAIName();

        AINameChanged?.Invoke(name);
    }

    public void OnColorChanged(int colorId)
    {
        _currentColor = colorId;
        UpdateEmojiList();
    }

    private void UpdateEmojiList()
    {
        var items = _service.GetEmojiItems(_currentColor);
        EmojiListChanged?.Invoke(items);
    }

    public void OnEmojiSelected(int index)
    {
        Sprite player = _service.SelectPlayerEmoji(_currentColor, index);
        if (player != null)
            PlayerAvatarChanged?.Invoke(player);

        Sprite ai = _service.EnsureValidAIEmoji();
        if (ai != null)
            AIAvatarChanged?.Invoke(ai);
    }

    public void OnStartPressed()
    {
        SceneManager.LoadScene("Main");
    }
}