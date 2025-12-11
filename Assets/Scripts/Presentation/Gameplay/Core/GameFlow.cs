using System;

public class GameFlow
{
    private readonly BoardState state;
    private readonly TurnState turn;
    private readonly WinChecker checker;

    public event Action<int, CellState> OnMoveApplied;

    public event Action<bool> OnTurnChanged;

    public event Action<CellState, WinLineView.WinLineType?, CellState[,]> OnGameOver;

    public GameFlow(BoardState state, TurnState turn, WinChecker checker)
    {
        this.state = state;
        this.turn = turn;
        this.checker = checker;
    }

    public void ProcessMove(int index)
    {
        if (!state.IsEmpty(index))
        {
            return;
        }

        var mark = turn.Current;

        state.Set(index, mark);
        OnMoveApplied?.Invoke(index, mark);

        var result = checker.Check(state.Data);

        if (result.IsGameOver)
        {
            WinLineView.WinLineType? line =
                result.WinLineIndex.HasValue
                ? (WinLineView.WinLineType?) result.WinLineIndex.Value
                : null;

            OnGameOver?.Invoke(result.Winner, line, state.Data);
            return;
        }

        turn.Next();

        OnTurnChanged?.Invoke(turn.IsPlayerTurn);
    }

    public void Reset()
    {
        state.Reset();
        turn.Reset();
        OnTurnChanged?.Invoke(true);
    }
}