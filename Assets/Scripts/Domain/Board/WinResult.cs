public readonly struct WinResult
{
    public bool IsGameOver { get; }
    public bool IsDraw { get; }
    public CellState Winner { get; }
    public int? WinLineIndex { get; }

    public WinResult(bool isGameOver, bool isDraw, CellState winner, int? winLineIndex)
    {
        IsGameOver = isGameOver;
        IsDraw = isDraw;
        Winner = winner;
        WinLineIndex = winLineIndex;
    }
}