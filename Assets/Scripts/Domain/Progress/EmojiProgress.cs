using System;

[Serializable]
public class EmojiProgress
{
    public int ColorId;
    public int EmojiId;
    public bool IsUnlocked;

    public EmojiProgress()
    { }

    public EmojiProgress(int colorId, int emojiId, bool unlocked = false)
    {
        ColorId = colorId;
        EmojiId = emojiId;
        IsUnlocked = unlocked;
    }
}