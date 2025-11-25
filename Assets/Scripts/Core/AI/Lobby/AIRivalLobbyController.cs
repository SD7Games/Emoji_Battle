using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AIRivalLobbyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _aiSign;
    [SerializeField] private List<EmojiData> _emojiDataByColor;
    [SerializeField] private PlayerLobbyController _playerLobbyController;

    private DissolveLobby _dissolve;
    private EmojiData _currentColorData;
    private int _currentEmojiIndex;

    private void Awake()
    {
        _dissolve = _aiSign.GetComponent<DissolveLobby>();
    }

    private void Start()
    {
        _playerLobbyController.OnCheckMatchAISign += HandlePlayerColorChange;
        InitializeAIEmoji();
    }

    private void OnDestroy()
    {
        _playerLobbyController.OnCheckMatchAISign -= HandlePlayerColorChange;
    }

    private void InitializeAIEmoji()
    {
        if (_emojiDataByColor == null || _emojiDataByColor.Count == 0)
            return;

        int playerColor = GD.Player.EmojiColor;
        int aiColor = GD.AI.EmojiColor;
        int aiIndex = GD.AI.EmojiIndex;

        EmojiData playerData = _emojiDataByColor.Find(d => d.ColorId == playerColor);
        EmojiData aiData = _emojiDataByColor.Find(d => d.ColorId == aiColor);

        if (aiData == null || aiData == playerData)
        {
            ChooseNewColorAndEmoji(playerData);
            return;
        }

        _currentColorData = aiData;
        _currentEmojiIndex = Mathf.Clamp(aiIndex, 0, _currentColorData.EmojiSprites.Count - 1);

        _aiSign.sprite = _currentColorData.EmojiSprites[_currentEmojiIndex];
        _dissolve?.PlayDissolve();
    }

    private void ChooseNewColorAndEmoji(EmojiData playerColorData)
    {
        if (_emojiDataByColor == null || _emojiDataByColor.Count == 0)
            return;

        List<EmojiData> available = new List<EmojiData>(_emojiDataByColor);

        if (playerColorData != null)
            available.Remove(playerColorData);

        if (available.Count == 0)
            return;

        _currentColorData = available[Random.Range(0, available.Count)];

        if (_currentColorData.EmojiSprites == null || _currentColorData.EmojiSprites.Count == 0)
            return;

        _currentEmojiIndex = Random.Range(0, _currentColorData.EmojiSprites.Count);

        _aiSign.sprite = _currentColorData.EmojiSprites[_currentEmojiIndex];
        _dissolve?.PlayDissolve();

        GD.AI.EmojiColor = _currentColorData.ColorId;
        GD.AI.EmojiIndex = _currentEmojiIndex;
        GD.Save();
    }

    private void HandlePlayerColorChange()
    {
        int playerColor = GD.Player.EmojiColor;
        EmojiData playerData = _emojiDataByColor.Find(colorData => colorData.ColorId == playerColor);

        if (_currentColorData == null || _currentColorData.ColorId == playerColor)
        {
            ChooseNewColorAndEmoji(playerData);
        }
    }
}
