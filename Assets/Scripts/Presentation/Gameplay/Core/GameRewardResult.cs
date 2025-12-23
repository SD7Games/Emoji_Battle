public readonly struct GameRewardResult
{
    public readonly bool EmojiUnlocked;
    public readonly bool AllEmojisUnlocked;

    public GameRewardResult(bool emojiUnlocked, bool allEmojisUnlocked)
    {
        EmojiUnlocked = emojiUnlocked;
        AllEmojisUnlocked = allEmojisUnlocked;
    }

    public static GameRewardResult None => new(false, false);
}