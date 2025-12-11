using System.Collections.Generic;
using UnityEngine;

public class HardStrategy : IAIStrategy
{
    public int TryGetMove(int[] board)
    {
        List<int> available = GameLogicHelper.GetAvailableMoves(board);
        if (available.Count == 0)
            return -1;

        // Always try to win
        int move = GameLogicHelper.FindWinningMove(board, 2);
        if (move >= 0)
            return move;

        // Almost always block the player 90%
        if (Random.value < 0.9f)
        {
            move = GameLogicHelper.FindWinningMove(board, 1);
            if (move >= 0)
                return move;
        }

        // Center has priority 80%
        if (board[4] == 0 && Random.value < 0.8f)
            return 4;

        // Look for best strategic corner 70%
        int[] corners = { 0, 2, 6, 8 };
        List<int> freeCorners = new List<int>();
        foreach (int i in corners)
            if (board[i] == 0)
                freeCorners.Add(i);

        if (freeCorners.Count > 0 && Random.value < 0.7f)
            return freeCorners[Random.Range(0, freeCorners.Count)];

        // Try to take a side near the center if it's occupied (smart move)
        if (board[4] != 0)
        {
            int[] sides = { 1, 3, 5, 7 };
            List<int> goodSides = new List<int>();
            foreach (int i in sides)
                if (board[i] == 0)
                    goodSides.Add(i);

            if (goodSides.Count > 0 && Random.value < 0.6f)
                return goodSides[Random.Range(0, goodSides.Count)];
        }

        // Otherwise, make a random move
        return available[Random.Range(0, available.Count)];
    }
}