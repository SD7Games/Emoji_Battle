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

        var rule = RewardRules.ByDifficulty(difficulty);

        if (progress.IsRangeFullyUnlocked(_sets, rule))
            return new(false, RewardBlockReason.AllUnlocked);

        bool unlocked = progress.UnlockNextInRange(_sets, rule);

        if (unlocked)
            GameDataService.I.Save();

        return unlocked
            ? new(true, RewardBlockReason.None)
            : new(false, RewardBlockReason.AllUnlocked);
    }

    public GameRewardResult RewardedOpened(bool hasInternet)
    {
        if (!hasInternet)
            return new GameRewardResult(false, RewardBlockReason.NoInternet);

        var progress = GameDataService.I.Data.Progress;

        if (IsAllUnlocked(progress))
            return new GameRewardResult(false, RewardBlockReason.AllUnlocked);

        bool unlocked = progress.UnlockRandomLocked(_sets);

        if (unlocked)
            GameDataService.I.Save();

        return unlocked
            ? new GameRewardResult(true, RewardBlockReason.None)
            : new GameRewardResult(false, RewardBlockReason.AllUnlocked);
    }

    private bool IsAllUnlocked(GameProgress progress)
    {
        foreach (var set in _sets)
        {
            for (int i = 0; i < set.EmojiSprites.Count; i++)
            {
                if (!progress.IsEmojiUnlocked(set.ColorId, i))
                    return false;
            }
        }

        return true;
    }
}