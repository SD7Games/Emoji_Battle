using System.Collections.Generic;

public sealed class GameRewardService
{
    private readonly List<EmojiData> _sets;

    public GameRewardService(List<EmojiData> sets)
    {
        _sets = sets;
    }

    public GameRewardResult OnWin(CellState winner, bool hasInternet)
    {
        if (winner != CellState.Player)
            return GameRewardResult.None;

        var progress = GameDataService.I.Data.Progress;

        if (IsAllUnlocked(progress))
            return new GameRewardResult(false, RewardBlockReason.AllUnlocked);

        if (!hasInternet)
            return new GameRewardResult(false, RewardBlockReason.NoInternet);

        bool unlocked = progress.UnlockNextGlobal(_sets);

        if (unlocked)
            GameDataService.I.Save();

        return unlocked
            ? new GameRewardResult(true, RewardBlockReason.None)
            : new GameRewardResult(false, RewardBlockReason.AllUnlocked);
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