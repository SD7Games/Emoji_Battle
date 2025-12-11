using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class ColorSwitcherView : MonoBehaviour
{
    public event Action<int> OnColorSelected;

    [SerializeField] private List<Button> _buttons;

    private void Awake()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            int id = i;
            _buttons[i].onClick.AddListener(() => OnColorSelected?.Invoke(id));
        }
    }
}