using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EmojiScrollView : MonoBehaviour
{
    [SerializeField] private EmojiButtonView _buttonPrefab;
    [SerializeField] private Transform _contentParent;

    public event Action<int> OnEmojiClicked;

    private readonly List<EmojiButtonView> _buttons = new();

    public void Fill(
        List<EmojiProgress> items,
        EmojiResolver resolver
    )
    {
        Rebuild(items, resolver);
    }

    private void Rebuild(
        List<EmojiProgress> items,
        EmojiResolver resolver
    )
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (_buttons[i] != null)
                Destroy(_buttons[i].gameObject);
        }

        _buttons.Clear();

        foreach (var data in items)
        {
            var btn = Instantiate(_buttonPrefab, _contentParent);

            btn.Bind(data.EmojiId);
            btn.Icon.sprite = resolver.Get(data.ColorId, data.EmojiId);

            btn.LockedOverlay.SetActive(!data.IsUnlocked);
            btn.LockIcon.SetActive(!data.IsUnlocked);
            btn.Button.interactable = data.IsUnlocked;

            btn.Button.onClick.AddListener(() =>
            {
                OnEmojiClicked?.Invoke(btn.EmojiIndex);
            });

            _buttons.Add(btn);
        }
    }
}