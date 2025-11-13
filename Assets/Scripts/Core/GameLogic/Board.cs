using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board
{
    private readonly CellState[,] _board = new CellState[3, 3];
    private readonly List<Button> _buttons;
    private Sprite _defaultSprite;

    private Sprite _playerEmoji;
    private Sprite _aiEmoji;

    public Board(List<Button> buttons, Sprite defaultSprite)
    {
        _buttons = buttons;
        _defaultSprite = defaultSprite;
        Reset();
    }

    public void LoadEmojis(List<EmojiData> emojiDataByColor)
    {
        if (emojiDataByColor == null || emojiDataByColor.Count == 0) return;

        string playerColor = GD.Player.EmojiColor;
        int playerIndex = GD.Player.EmojiIndex;
        string aiColor = GD.AI.EmojiColor;
        int aiIndex = GD.AI.EmojiIndex;

        EmojiData playerData = emojiDataByColor.Find(colorData => colorData.ColorName == playerColor);
        EmojiData aiData = emojiDataByColor.Find(colorData => colorData.ColorName == aiColor);

        if (playerData != null && playerData.EmojiSprites.Count > 0)
            _playerEmoji = playerData.EmojiSprites[Mathf.Clamp(playerIndex, 0, playerData.EmojiSprites.Count - 1)];

        if (aiData != null && aiData.EmojiSprites.Count > 0)
            _aiEmoji = aiData.EmojiSprites[Mathf.Clamp(aiIndex, 0, aiData.EmojiSprites.Count - 1)];
    }

    public void SetCell(int index, CellState state, Sprite customSprite = null)
    {
        int row = index / 3;
        int col = index % 3;
        _board[row, col] = state;

        var button = _buttons[index];
        var image = button.image;

        Sprite sprite = customSprite ??
                        (state == CellState.Player ? _playerEmoji :
                         state == CellState.AI ? _aiEmoji :
                         _defaultSprite);

        image.sprite = sprite;
        SetAlpha(image, 1f);
        button.interactable = false;

        var dissolve = button.GetComponent<UIDissolve>();
        if (dissolve != null)
            dissolve.PlayDissolve();
    }

    public bool IsCellEmpty(int index)
    {
        int row = index / 3;
        int col = index % 3;
        return _board[row, col] == CellState.Empty;
    }

    public void Reset()
    {
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                _board[r, c] = CellState.Empty;
                int index = r * 3 + c;

                var image = _buttons[index].image;
                image.enabled = true;
                image.sprite = _defaultSprite;
                SetAlpha(image, 0f);
                _buttons[index].interactable = true;
            }
        }
    }

    public void DisableAll(CellState[,] boardState)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            int row = i / 3;
            int col = i % 3;

            var button = _buttons[i];
            var image = button.image;
            button.interactable = false;

            if (boardState[row, col] == CellState.Empty)
                SetAlpha(image, 0f);
        }
    }

    public CellState[,] GetBoardState() => _board;

    public int[] GetBoardAsIntArray()
    {
        int[] result = new int[9];
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                int index = r * 3 + c;
                result[index] = _board[r, c] switch
                {
                    CellState.Player => 1,
                    CellState.AI => 2,
                    _ => 0
                };
            }
        }
        return result;
    }

    private void SetAlpha(Image image, float alpha)
    {
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
