using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmojiData", menuName = "Assets/Emoji Data")]
public class EmojiData : ScriptableObject
{
    [Header("Color info")]
    public int ColorId;

    [Header("Emoji list of this color")]
    public List<Sprite> EmojiSprites;

    public Sprite GetEmojiByIndex(int index)
    {
        if (EmojiSprites == null || EmojiSprites.Count == 0)
            return null;

        if (index < 0 || index >= EmojiSprites.Count)
            index = 0;

        return EmojiSprites[index];
    }

    public Sprite GetRandomEmoji()
    {
        if (EmojiSprites == null || EmojiSprites.Count == 0)
            return null;

        return EmojiSprites[Random.Range(0, EmojiSprites.Count)];
    }
}
