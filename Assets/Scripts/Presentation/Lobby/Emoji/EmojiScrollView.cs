using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EmojiScrollView : MonoBehaviour
{
    [SerializeField] private EmojiButtonView _buttonPrefab;
    [SerializeField] private Transform _contentParent;

    public event Action<int> OnEmojiClicked;

    private readonly List<EmojiButtonView> _buttons = new();
    private bool _initialized = false;

    private const int MAX_COUNT = 87;

    public void Fill(EmojiViewData[] items)
    {
        if (!_initialized)
        {
            CreateButtons();
            _initialized = true;
        }

        UpdateButtons(items);
    }

    private void CreateButtons()
    {
        for (int i = 0; i < MAX_COUNT; i++)
        {
            EmojiButtonView btn = Instantiate(_buttonPrefab, _contentParent);

            int index = i;
            btn.Button.onClick.AddListener(() =>
            {
                OnEmojiClicked?.Invoke(index);
            });

            _buttons.Add(btn);
        }
    }

    private void UpdateButtons(EmojiViewData[] items)
    {
        int count = items.Length;

        for (int i = 0; i < MAX_COUNT; i++)
        {
            var btn = _buttons[i];

            if (i >= count)
            {
                btn.gameObject.SetActive(false);
                continue;
            }

            btn.gameObject.SetActive(true);

            var data = items[i];

            btn.Icon.sprite = data.Sprite != null ? data.Sprite : null;

            bool unlocked = data.Unlocked;

            btn.LockedOverlay.SetActive(!unlocked);
            btn.LockIcon.SetActive(!unlocked);
            btn.Button.interactable = unlocked;
        }
    }
}