public class GameSession
{
    private readonly GameFlow flow;
    private readonly TurnState turn;
    private readonly WinLineView lines;
    private readonly BoardView board;

    public GameSession(GameFlow flow, TurnState turn, WinLineView lines, BoardView board)
    {
        this.flow = flow;
        this.turn = turn;
        this.lines = lines;
        this.board = board;
    }

    public void Restart()
    {
        flow.Reset();
        turn.Reset();
        lines.HideAllLines();
        board.ResetView();
        board.SetInteractable(true);
    }
}