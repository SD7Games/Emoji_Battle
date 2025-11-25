using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EmojiDataSetter : MonoBehaviour
{
    [Header("Emoji Data Setter")]
    [SerializeField] private EmojiData _currentData;
    [SerializeField] private List<EmojiData> _emojiDataByColor;

    public EmojiData CurrentData => _currentData;
    public List<EmojiData> AllData => _emojiDataByColor;

    private void Start()
    {
        if (_currentData == null && _emojiDataByColor.Count > 0)
            _currentData = _emojiDataByColor[0];
    }

    public void SetEmojiData(EmojiData newData)
    {
        if (newData == null) return;

        _currentData = newData;

        GD.Player.EmojiColor = _currentData.ColorId;

        GD.Save();
    }

    public Sprite GetEmojiByIndex(int index)
    {
        if (_currentData == null)
            return null;

        return _currentData.GetEmojiByIndex(index);
    }

    public Sprite GetRandomEmoji()
    {
        if (_currentData == null)
            return null;

        return _currentData.GetRandomEmoji();
    }
}
