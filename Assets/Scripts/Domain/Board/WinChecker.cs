public class WinChecker
{
    public static readonly int[][] Lines =
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

    public WinResult Check(CellState[,] board)
    {
        for (int i = 0; i < Lines.Length; i++)
        {
            int a = Lines[i][0];
            int b = Lines[i][1];
            int c = Lines[i][2];

            var A = board[a / 3, a % 3];
            var B = board[b / 3, b % 3];
            var C = board[c / 3, c % 3];

            if (A != CellState.Empty && A == B && B == C)
            {
                return new WinResult(true, false, A, i);
            }
        }

        bool anyLineCanStillWin = false;

        foreach (var line in Lines)
        {
            bool hasPlayer = false;
            bool hasAI = false;

            foreach (int idx in line)
            {
                var cell = board[idx / 3, idx % 3];

                if (cell == CellState.Player) hasPlayer = true;
                if (cell == CellState.AI) hasAI = true;
            }

            if (!(hasPlayer && hasAI))
            {
                anyLineCanStillWin = true;
                break;
            }
        }

        if (!anyLineCanStillWin)
        {
            return new WinResult(true, true, CellState.Empty, null);
        }

        foreach (var cell in board)
        {
            if (cell == CellState.Empty)
            {
                return new WinResult(false, false, CellState.Empty, null);
            }
        }

        return new WinResult(true, true, CellState.Empty, null);
    }
}