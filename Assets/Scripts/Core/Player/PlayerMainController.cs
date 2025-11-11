using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class PlayerMainController : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private Image _playerSprite;
    [SerializeField] private List<EmojiData> _emojiDataByColor;

    private void Start()
    {
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        _playerName.text = GD.Player.Name;

        string colorName = GD.Player.EmojiColor;
        int emojiIndex = GD.Player.EmojiIndex;

        EmojiData colorData = _emojiDataByColor.Find(data => data.ColorName == colorName);
        if (colorData == null) return;

        if (emojiIndex >= 0 && emojiIndex < colorData.EmojiSprites.Count)
        {
            _playerSprite.sprite = colorData.EmojiSprites[emojiIndex];
        }
    }
}
