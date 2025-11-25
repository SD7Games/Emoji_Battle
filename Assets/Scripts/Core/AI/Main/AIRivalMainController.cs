using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class AIRivalMainController : MonoBehaviour
{
    [SerializeField] private Image _aiSign;
    [SerializeField] private List<EmojiData> _emojiDataByColor;

    private void Start()
    {
        LoadAIData();
    }

    private void LoadAIData()
    {
        int colorId = GD.AI.EmojiColor;
        int emojiIndex = GD.AI.EmojiIndex;

        EmojiData colorData = _emojiDataByColor.Find(d => d.ColorId == colorId);
        if (colorData == null || colorData.EmojiSprites.Count == 0)
            return;

        int safeIndex = Mathf.Clamp(emojiIndex, 0, colorData.EmojiSprites.Count - 1);
        _aiSign.sprite = colorData.EmojiSprites[safeIndex];
    }
}
