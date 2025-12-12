using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputController
{
    private readonly IReadOnlyList<Button> _buttons;
    private bool _isBlocked;

    public event Action<int> OnCellClicked;

    public InputController(IReadOnlyList<Button> buttons)
    {
        this._buttons = buttons ?? throw new ArgumentNullException(nameof(buttons));

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    private void OnButtonClick(int index)
    {
        if (_isBlocked)
        {
            return;
        }

        var button = _buttons[index];
        if (button == null || !button.interactable)
        {
            return;
        }

        OnCellClicked?.Invoke(index);
    }
}