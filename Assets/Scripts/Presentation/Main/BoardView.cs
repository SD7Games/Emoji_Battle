using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;

    private Sprite player;
    private Sprite ai;

    public IReadOnlyList<Button> Buttons => buttons;

    public event Action<int> OnCellPressed;

    private void Awake()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnCellPressed?.Invoke(index));
        }
    }

    public void AssignSprites(Sprite p, Sprite a)
    {
        player = p;
        ai = a;
    }

    public void OnMoveApplied(int index, CellState state)
    {
        var btn = buttons[index];
        btn.image.sprite = state == CellState.Player ? player : ai;
        btn.interactable = false;
        btn.image.color = Color.white;

        if (btn.TryGetComponent<DissolveMain>(out var d))
            d.PlayDissolve();
    }

    public void ResetView()
    {
        foreach (var btn in buttons)
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
        foreach (var btn in buttons)
            btn.interactable = value;
    }

    public void DisableAfterGameOver(CellState[,] board)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var btn = buttons[i];
            var cell = board[i / 3, i % 3];

            if (cell == CellState.Empty)
                btn.image.color = new Color(1, 1, 1, 0);

            btn.interactable = false;
        }
    }
}