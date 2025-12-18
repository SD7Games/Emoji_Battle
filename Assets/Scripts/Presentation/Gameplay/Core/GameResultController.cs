
public sealed class GameResultController
{
    private readonly WinLineView _lines;
    private readonly GameRewardService _rewards;

    public GameResultController(
        WinLineView lines,
        GameRewardService rewards
    )
    {
        _lines = lines;
        _rewards = rewards;
    }

    public void HandleGameOver(
        CellState winner,
        WinLineView.WinLineType? line
    )
    {
        _rewards.OnWin(winner);

        if (line.HasValue)
        {
            _lines.ShowWinLine(
                line.Value,
                () => ShowResultPopup(winner)
            );
        }
        else
        {
            ShowResultPopup(winner);
        }
    }

    private void ShowResultPopup(CellState winner)
    {
        PopupId id = winner switch
        {
            CellState.Player => PopupId.Victory,
            CellState.AI => PopupId.Defeat,
            _ => PopupId.Draw
        };

        PopupService.I.Show(id);
    }
}