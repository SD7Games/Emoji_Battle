using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;

    private Sprite _player;
    private Sprite _ai;

    public IReadOnlyList<Button> Buttons => _buttons;

    public event Action<int> OnCellPressed;

    private void Awake()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            int index = i;
            _buttons[i].onClick.AddListener(() => OnCellPressed?.Invoke(index));
        }
    }

    public void AssignSprites(Sprite p, Sprite a)
    {
        _player = p;
        _ai = a;
    }

    public void OnMoveApplied(int index, CellState state)
    {
        var btn = _buttons[index];
        btn.image.sprite = state == CellState.Player ? _player : _ai;
        btn.interactable = false;
        btn.image.color = Color.white;

        if (btn.TryGetComponent<DissolveMain>(out var d))
            d.PlayDissolve();
    }

    public void ResetView()
    {
        foreach (var btn in _buttons)
        {
            btn.image.sprite = null;
            btn.image.color = new Color(1, 1, 1, 0);
            btn.interactable = true;

            if (btn.TryGetComponent<DissolveMain>(out var d))
                d.ResetDissolve();
        }
    }

    public void SetInteractable(bool value)
    {
        foreach (var btn in _buttons)
            btn.interactable = value;
    }

    public void DisableAfterGameOver(CellState[,] board)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            var btn = _buttons[i];
            var cell = board[i / 3, i % 3];

            if (cell == CellState.Empty)
                btn.image.color = new Color(1, 1, 1, 0);

            btn.interactable = false;
        }
    }
}