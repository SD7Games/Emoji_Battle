using System;

[Serializable]
public class EmojiProgress
{
    public int ColorId;
    public int EmojiId;
    public bool IsUnlocked;
    public int UnlockOrder;

    public EmojiProgress(int colorId, int emojiId)
    {
        ColorId = colorId;
        EmojiId = emojiId;
        IsUnlocked = false;
        UnlockOrder = -1;
    }
}