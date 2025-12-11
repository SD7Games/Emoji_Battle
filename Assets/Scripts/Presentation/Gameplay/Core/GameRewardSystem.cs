using System.Collections.Generic;

public class GameRewardService
{
    private readonly List<EmojiData> _emojis;

    public GameRewardService(List<EmojiData> emojis)
    {
        _emojis = emojis;
    }

    public void OnWin(CellState winner)
    {
        if (winner != CellState.Player)
            return;

        var progress = GameDataService.I.Data.Progress;

        if (progress.UnlockNextGlobal(_emojis))
        {
            GameDataService.I.Save();
        }
    }
}