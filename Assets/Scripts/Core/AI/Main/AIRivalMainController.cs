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
        string colorName = GD.AI.EmojiColor;
        int emojiIndex = GD.AI.EmojiIndex;

        EmojiData colorData = _emojiDataByColor.Find(d => d.ColorName == colorName);
        if (colorData == null) return;

        if (emojiIndex >= 0 && emojiIndex < colorData.EmojiSprites.Count)
        {
            _aiSign.sprite = colorData.EmojiSprites[emojiIndex];
        }
    }
}
