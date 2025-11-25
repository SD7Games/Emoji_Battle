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

    private int _currentColorId;
    private int _emojiCountForThisColor;

    private void Start()
    {
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        _playerName.text = GD.Player.Name;

        _currentColorId = GD.Player.EmojiColor;
        int emojiIndex = GD.Player.EmojiIndex;

        EmojiData colorData = _emojiDataByColor.Find(d => d.ColorId == _currentColorId);
        if (colorData == null)
        {
            Debug.LogError($"PlayerMainController: EmojiData not found for color ID {_currentColorId}");
            return;
        }

        _emojiCountForThisColor = colorData.EmojiSprites.Count;

        if (emojiIndex >= 0 && emojiIndex < _emojiCountForThisColor)
            _playerSprite.sprite = colorData.EmojiSprites[emojiIndex];
    }

    public void OnPlayerWin()
    {
        GD.PlayerProgress.Wins++;

        bool opened = GD.PlayerProgress.UnlockNextLocked(_currentColorId, _emojiCountForThisColor);

        if (opened)
        {
            GD.PlayerProgress.LootBoxes++;
            Debug.Log($"NEW EMOJI UNLOCKED! LootBoxes: {GD.PlayerProgress.LootBoxes}");
        }
        else
        {
            Debug.Log("All emoji are already unlocked.");
        }

        GD.Save();
    }
}
