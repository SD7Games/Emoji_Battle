using System;

public class GameFlow
{
    private readonly BoardState _state;
    private readonly TurnState _turn;
    private readonly WinChecker _checker;

    public event Action<int, CellState> OnMoveApplied;

    public event Action<bool> OnTurnChanged;

    public event Action<CellState, WinLineView.WinLineType?, CellState[,]> OnGameOver;

    public GameFlow(BoardState state, TurnState turn, WinChecker checker)
    {
        _state = state;
        _turn = turn;
        _checker = checker;
    }

    public void ProcessMove(int index)
    {
        if (!_state.IsEmpty(index))
        {
            return;
        }

        var mark = _turn.Current;

        _state.Set(index, mark);
        OnMoveApplied?.Invoke(index, mark);

        var result = _checker.Check(_state.Data);

        if (result.IsGameOver)
        {
            WinLineView.WinLineType? line =
                result.WinLineIndex.HasValue
                ? (WinLineView.WinLineType?) result.WinLineIndex.Value
                : null;

            OnGameOver?.Invoke(result.Winner, line, _state.Data);
            return;
        }

        _turn.Next();

        OnTurnChanged?.Invoke(_turn.IsPlayerTurn);
    }

    public void Reset()
    {
        _state.Reset();
        _turn.Reset();
        OnTurnChanged?.Invoke(true);
    }
}