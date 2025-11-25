using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class ContentScrollController : MonoBehaviour
{
    [SerializeField] private GameObject _emojiButtonPrefab;
    [SerializeField] private Transform _contentParent;

    public event Action<Sprite> OnEmojiSelected;
    public event Action OnGenerationComplete;

    public void SetEmojiData(EmojiData emojiData, PlayerProgress playerProgress, Action<int> onEmojiSelected)
    {
        if (emojiData == null || emojiData.EmojiSprites == null)
            return;

        foreach (Transform child in _contentParent)
            Destroy(child.gameObject);

        int total = emojiData.EmojiSprites.Count;
        var sortedIndexes = playerProgress.GetSortedEmojiIndexes(emojiData.ColorId, total);

        foreach (int emojiIndex in sortedIndexes)
        {
            bool unlocked = playerProgress.IsEmojiUnlocked(emojiData.ColorId, emojiIndex);
            Sprite sprite = emojiData.EmojiSprites[emojiIndex];

            var buttonGO = Instantiate(_emojiButtonPrefab, _contentParent);

            var image = buttonGO.GetComponentInChildren<Image>();
            image.sprite = sprite;

            var lockedText = buttonGO.GetComponentInChildren<TMP_Text>(true);

            var button = buttonGO.GetComponent<Button>();
           
            lockedText.gameObject.SetActive(!unlocked);
            button.interactable = unlocked;

            int safeIndex = emojiIndex;

            button.onClick.AddListener(() =>
            {
                if (!unlocked) return;

                onEmojiSelected?.Invoke(safeIndex);
                OnEmojiSelected?.Invoke(sprite);
            });
        }

        OnGenerationComplete?.Invoke();
    }
}
