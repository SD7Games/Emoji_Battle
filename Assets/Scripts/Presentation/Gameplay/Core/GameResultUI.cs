public class GameResultUI
{
    private readonly WinLineView _lines;
    private readonly BoardView _board;

    public GameResultUI(WinLineView lines, BoardView board)
    {
        _lines = lines;
        _board = board;
    }

    public void Show(CellState winner, WinLineView.WinLineType? line, CellState[,] final)
    {
        if (line.HasValue)
            _lines.ShowWinLine(line.Value);

        _board.DisableAfterGameOver(final);
    }
}