using System;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ContentScrollController : MonoBehaviour
{
    [SerializeField] private GameObject _emojiButtonPrefab;
    [SerializeField] private Transform _contentParent;

    public event Action<Sprite> OnEmojiSelected;
    public event Action OnGenerationComplete;

    public void SetEmojiData(EmojiData emojiData)
    {
        if (emojiData == null || emojiData.EmojiSprites == null)
            return;

        foreach (Transform child in _contentParent)
            Destroy(child.gameObject);

        foreach (var sprite in emojiData.EmojiSprites)
        {
            var buttonGO = Instantiate(_emojiButtonPrefab, _contentParent);
            var image = buttonGO.GetComponentInChildren<Image>();
            image.sprite = sprite;

            var button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                int index = emojiData.EmojiSprites.IndexOf(sprite);

                GD.Player.EmojiIndex = index;
                GD.Save();

                OnEmojiSelected?.Invoke(sprite);
            });
        }

        OnGenerationComplete?.Invoke();
    }
}
