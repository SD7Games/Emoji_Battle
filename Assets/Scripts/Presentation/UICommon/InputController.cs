using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputController
{
    private readonly IReadOnlyList<Button> buttons;
    private bool isBlocked;

    public event Action<int> OnCellClicked;

    public InputController(IReadOnlyList<Button> buttons)
    {
        this.buttons = buttons ?? throw new ArgumentNullException(nameof(buttons));

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    private void OnButtonClick(int index)
    {
        if (isBlocked)
        {
            return;
        }

        var button = buttons[index];
        if (button == null || !button.interactable)
        {
            return;
        }

        OnCellClicked?.Invoke(index);
    }
}