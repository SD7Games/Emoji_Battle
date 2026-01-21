public enum RewardBlockReason
{
    None,
    NoInternet,
    AllUnlocked,
    DifficultyCompleted
}

public readonly struct GameRewardResult
{
    public readonly bool EmojiUnlocked;
    public readonly RewardBlockReason BlockReason;

    public GameRewardResult(bool unlocked, RewardBlockReason reason)
    {
        EmojiUnlocked = unlocked;
        BlockReason = reason;
    }

    public static GameRewardResult None =>
        new(false, RewardBlockReason.None);
}