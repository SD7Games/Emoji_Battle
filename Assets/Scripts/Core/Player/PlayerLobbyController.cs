using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PlayerLobbyController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Image _playerSign;
    [SerializeField] private ContentScrollController _contentScroll;
    [SerializeField] private EmojiDataSetter _emojiSetter;

    private EmojiData _currentEmojiData;
    private Dictionary<int, int> _totalEmojisPerColor = new();

    public event Action OnCheckMatchAISign;

    private void Start()
    {
        foreach (var data in _emojiSetter.AllData)
            _totalEmojisPerColor[data.ColorId] = data.EmojiSprites.Count;

        InitializeDefaultProgress();
        LoadViewFromProgress();
    }

    private void InitializeDefaultProgress()
    {
        if (GD.PlayerProgress.HasAnyProgress())
            return;

        GD.PlayerProgress.UnlockFirstNAllColors(_totalEmojisPerColor, 3);
        GD.SaveProgress();
    }

    private void LoadViewFromProgress()
    {
        int savedColor = GD.Player.EmojiColor;
        int savedIndex = GD.Player.EmojiIndex;

        _currentEmojiData = _emojiSetter.AllData.Find(d => d.ColorId == savedColor)
                           ?? _emojiSetter.AllData[0];

        if (_currentEmojiData == null) return;

        savedIndex = Mathf.Clamp(savedIndex, 0, _currentEmojiData.EmojiSprites.Count - 1);

        _emojiSetter.SetEmojiData(_currentEmojiData);
        _contentScroll.SetEmojiData(_currentEmojiData, GD.PlayerProgress, OnEmojiSelected);

        _playerSign.sprite = _currentEmojiData.EmojiSprites[savedIndex];
    }

    private void OnEmojiSelected(int emojiIndex)
    {
        GD.Player.EmojiColor = _currentEmojiData.ColorId;
        GD.Player.EmojiIndex = emojiIndex;
        GD.Save();

        _playerSign.sprite = _currentEmojiData.EmojiSprites[emojiIndex];
        OnCheckMatchAISign?.Invoke();
    }

    public void SetEmojiData(EmojiData newData)
    {
        if (newData == null) return;

        _currentEmojiData = newData;
        _emojiSetter.SetEmojiData(newData);

        GD.Player.EmojiColor = newData.ColorId;
        GD.Player.EmojiIndex = Mathf.Clamp(GD.Player.EmojiIndex, 0, newData.EmojiSprites.Count - 1);
        GD.Save();

        _contentScroll.SetEmojiData(newData, GD.PlayerProgress, OnEmojiSelected);
        _playerSign.sprite = newData.EmojiSprites[GD.Player.EmojiIndex];

        OnCheckMatchAISign?.Invoke();
    }
}
