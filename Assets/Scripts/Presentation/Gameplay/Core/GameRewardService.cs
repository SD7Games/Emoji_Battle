using System.Collections.Generic;

public sealed class GameRewardService
{
    private readonly List<EmojiData> _sets;

    public GameRewardService(List<EmojiData> sets)
    {
        _sets = sets;
    }

    public GameRewardResult OnWin(CellState winner, AIStrategyType difficulty, bool hasInternet)
    {
        if (winner != CellState.Player)
            return GameRewardResult.None;

        if (!hasInternet)
            return new(false, RewardBlockReason.NoInternet);

        var progress = GameDataService.I.Data.Progress;

        if (progress.IsAllUnlocked(_sets))
            return new(false, RewardBlockReason.AllUnlocked);

        var rule = RewardRules.ByDifficulty(difficulty);

        bool unlocked = progress.UnlockNextWinByDifficulty(
            difficulty, _sets, rule);

        if (!unlocked)
            return new(false, RewardBlockReason.DifficultyCompleted);

        GameDataService.I.Save();
        return new(true, RewardBlockReason.None);
    }

    public GameRewardResult RewardedOpened(bool hasInternet)
    {
        if (!hasInternet)
            return new(false, RewardBlockReason.NoInternet);

        var progress = GameDataService.I.Data.Progress;

        if (progress.IsAllUnlocked(_sets))
            return new(false, RewardBlockReason.AllUnlocked);

        bool unlocked = progress.UnlockRandomLocked(_sets);

        if (unlocked)
            GameDataService.I.Save();

        return unlocked
            ? new(true, RewardBlockReason.None)
            : new(false, RewardBlockReason.AllUnlocked);
    }
}