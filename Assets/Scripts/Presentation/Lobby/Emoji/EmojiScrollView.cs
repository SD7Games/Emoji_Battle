using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EmojiScrollView : MonoBehaviour
{
    [SerializeField] private EmojiButtonView _buttonPrefab;
    [SerializeField] private Transform _contentParent;

    public event Action<int> OnEmojiClicked;

    private readonly List<EmojiButtonView> _buttons = new();
    private bool _initialized;

    public void Fill(
        List<EmojiProgress> items,
        EmojiResolver resolver
    )
    {
        if (!_initialized)
        {
            CreateButtons(items.Count);
            _initialized = true;
        }

        UpdateButtons(items, resolver);
    }

    private void CreateButtons(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var btn = Instantiate(_buttonPrefab, _contentParent);

            btn.Button.onClick.AddListener(() =>
            {
                OnEmojiClicked?.Invoke(btn.EmojiIndex);
            });

            _buttons.Add(btn);
        }
    }

    private void UpdateButtons(
        List<EmojiProgress> items,
        EmojiResolver resolver
    )
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            var btn = _buttons[i];
            var data = items[i];

            btn.gameObject.SetActive(true);
            btn.Bind(data.EmojiId);
            btn.Icon.sprite = resolver.Get(data.ColorId, data.EmojiId);

            btn.LockedOverlay.SetActive(!data.IsUnlocked);
            btn.LockIcon.SetActive(!data.IsUnlocked);
            btn.Button.interactable = data.IsUnlocked;
        }
    }
}