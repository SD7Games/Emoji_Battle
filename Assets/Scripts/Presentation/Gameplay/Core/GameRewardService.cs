using System.Collections.Generic;

public sealed class GameRewardService
{
    private readonly List<EmojiData> _sets;

    public GameRewardService(List<EmojiData> sets)
    {
        _sets = sets;
    }

    public GameRewardResult OnWin(CellState winner)
    {
        if (winner != CellState.Player)
            return GameRewardResult.None;

        var progress = GameDataService.I.Data.Progress;

        if (IsAllUnlocked(progress))
            return new GameRewardResult(false, true);

        bool unlocked = progress.UnlockNextGlobal(_sets);
        bool allUnlockedAfter = IsAllUnlocked(progress);

        if (unlocked)
            GameDataService.I.Save();

        return new GameRewardResult(unlocked, allUnlockedAfter);
    }

    public GameRewardResult OnLootBoxOpened()
    {
        var progress = GameDataService.I.Data.Progress;

        if (IsAllUnlocked(progress))
            return new GameRewardResult(false, true);

        bool unlocked = progress.UnlockRandomLocked(_sets);
        bool allUnlockedAfter = IsAllUnlocked(progress);

        if (unlocked)
            GameDataService.I.Save();

        return new GameRewardResult(unlocked, allUnlockedAfter);
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