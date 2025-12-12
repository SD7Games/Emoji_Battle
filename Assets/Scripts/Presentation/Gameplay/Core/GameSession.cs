public class GameSession
{
    private readonly GameFlow _flow;
    private readonly TurnState _turn;
    private readonly WinLineView _lines;
    private readonly BoardView _board;

    public GameSession(GameFlow flow, TurnState turn, WinLineView lines, BoardView board)
    {
        _flow = flow;
        _turn = turn;
        _lines = lines;
        _board = board;
    }

    public void Restart()
    {
        _flow.Reset();
        _turn.Reset();
        _lines.HideAllLines();
        _board.ResetView();
        _board.SetInteractable(true);
    }
}