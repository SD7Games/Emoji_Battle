using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class AIRivalMoveController : MonoBehaviour
{
    private IAIStrategy _strategy;
    private InputController _input;
    private Board _board;
    private WinChecker _winChecker;

    public void Initialize(InputController input, Board board)
    {
        _input = input;
        _board = board;

        _winChecker = new WinChecker();

        LoadStrategy();
    }

    private void LoadStrategy()
    {
        string strategyName = GD.AI.Strategy;

        _strategy = strategyName switch
        {
            "Easy" => new EasyStrategy(),
            "Normal" => new NormalStrategy(),
            "Hard" => new HardStrategy(),
            _ => new EasyStrategy(),
        };
    }

    public void MakeMove()
    {
        CellState[,] boardState = _board.GetBoardState();
        if (_winChecker.IsGameOver(boardState, out _, out _))
        {
            return;
        }

        StartCoroutine(MakeMoveRoutine());
    }

    private IEnumerator MakeMoveRoutine()
    {
        _input.BlockInput();

        yield return new WaitForSeconds(0.4f);

        int[] boardState = _board.GetBoardAsIntArray();
        int moveIndex = _strategy.TryGetMove(boardState);

        if (moveIndex >= 0)
        {
            _input.SimulateClick(moveIndex);
            yield return new WaitForSeconds(0.4f);
        }

        _input.AllowInput();
    }
}
