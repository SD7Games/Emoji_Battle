using System.Collections.Generic;
using UnityEngine;

public class EasyStrategy : IAIStrategy
{
    private static readonly int[] PreferredOrder = { 4, 0, 2, 6, 8, 1, 3, 5, 7 };

    public int TryGetMove(int[] board)
    {
        List<int> available = GameLogicHelper.GetAvailableMoves(board);
        if (available.Count == 0)
            return -1;

        //  Try to win 50%
        if (Random.value < 0.5f)
        {
            int winMove = GameLogicHelper.FindWinningMove(board, 2);
            if (winMove >= 0)
                return winMove;
        }

        // Try to block the player 40%
        if (Random.value < 0.4f)
        {
            int blockMove = GameLogicHelper.FindWinningMove(board, 1);
            if (blockMove >= 0)
                return blockMove;
        }

        // Take the center if available 30%
        if (board[4] == 0 && Random.value < 0.3f)
            return 4;

        // Try to take a corner 30%
        int[] corners = { 0, 2, 6, 8 };

        List<int> freeCorners = new List<int>();

        foreach (int i in corners)
            if (board[i] == 0)
                freeCorners.Add(i);

        if (freeCorners.Count > 0 && Random.value < 0.3f)
            return freeCorners[Random.Range(0, freeCorners.Count)];

        // Otherwise, make a random move
        return available[Random.Range(0, available.Count)];
    }
}