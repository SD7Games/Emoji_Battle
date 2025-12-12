using System.Collections.Generic;

public static class GameLogicHelper
{
    private static readonly int[][] _wins = WinChecker.Lines;

    public static CellState[,] To2DBoard(int[] board)
    {
        CellState[,] b = new CellState[BoardState.SIZE, BoardState.SIZE];

        for (int i = 0; i < board.Length; i++)
        {
            int value = board[i];
            b[i / BoardState.SIZE, i % BoardState.SIZE] = value switch
            {
                1 => CellState.Player,
                2 => CellState.AI,
                _ => CellState.Empty
            };
        }

        return b;
    }

    public static bool CheckWinner(CellState[,] board, out CellState winner, out int? winLineIndex)
    {
        winner = CellState.Empty;
        winLineIndex = null;

        for (int i = 0; i < _wins.Length; i++)
        {
            int a = _wins[i][0], b = _wins[i][1], c = _wins[i][2];

            var A = board[a / 3, a % 3];
            var B = board[b / 3, b % 3];
            var C = board[c / 3, c % 3];

            if (A != CellState.Empty && A == B && B == C)
            {
                winner = A;
                winLineIndex = i;
                return true;
            }
        }

        foreach (var cell in board)
            if (cell == CellState.Empty)
                return false;

        winner = CellState.Empty;
        return true;
    }

    public static int FindWinningMove(int[] board, int mark)
    {
        for (int i = 0; i < _wins.Length; i++)
        {
            int emptyIndex = -1;
            int countMark = 0;

            for (int j = 0; j < 3; j++)
            {
                int idx = _wins[i][j];
                if (board[idx] == mark)
                    countMark++;
                else if (board[idx] == 0)
                    emptyIndex = idx;
            }

            if (countMark == 2 && emptyIndex != -1)
                return emptyIndex;
        }

        return -1;
    }

    public static List<int> GetAvailableMoves(int[] board)
    {
        List<int> moves = new List<int>();

        for (int i = 0; i < board.Length; i++)
            if (board[i] == 0)
                moves.Add(i);

        return moves;
    }
}