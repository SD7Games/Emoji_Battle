public class WinChecker
{
    private static readonly int[][] Lines = new int[][]
    {
        new[] { 0, 1, 2 },
        new[] { 3, 4, 5 },
        new[] { 6, 7, 8 },
        new[] { 0, 3, 6 },
        new[] { 1, 4, 7 },
        new[] { 2, 5, 8 },
        new[] { 0, 4, 8 },
        new[] { 2, 4, 6 }
    };

    public bool IsGameOver(CellState[,] board, out CellState winner, out WinLineView.WinLineType? winLine)
    {
        winner = CellState.Empty;
        winLine = null;

        for (int i = 0; i < Lines.Length; i++)
        {
            int a = Lines[i][0];
            int b = Lines[i][1];
            int c = Lines[i][2];

            CellState A = board[a / 3, a % 3];
            CellState B = board[b / 3, b % 3];
            CellState C = board[c / 3, c % 3];

            if (A != CellState.Empty && A == B && B == C)
            {
                winner = A;
                winLine = (WinLineView.WinLineType) i;
                return true;
            }
        }

        foreach (var line in Lines)
        {
            bool hasPlayer = false;
            bool hasAI = false;

            foreach (int idx in line)
            {
                var state = board[idx / 3, idx % 3];
                if (state == CellState.Player) hasPlayer = true;
                else if (state == CellState.AI) hasAI = true;
            }

            if (!(hasPlayer && hasAI))
                return false;
        }

        winner = CellState.Empty;
        return true;
    }
}
