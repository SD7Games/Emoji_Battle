using UnityEngine;

public class TurnState : MonoBehaviour
{
    private bool isPlayerTurn = true;

    public bool IsPlayerTurn => isPlayerTurn;
    public CellState Current => isPlayerTurn ? CellState.Player : CellState.AI;

    public void Next() => isPlayerTurn = !isPlayerTurn;

    public void Reset() => isPlayerTurn = true;
}