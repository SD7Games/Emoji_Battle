using UnityEngine;

public class TurnState : MonoBehaviour
{
    private bool _isPlayerTurn = true;

    public bool IsPlayerTurn => _isPlayerTurn;
    public CellState Current => _isPlayerTurn ? CellState.Player : CellState.AI;

    public void Next() => _isPlayerTurn = !_isPlayerTurn;

    public void Reset() => _isPlayerTurn = true;
}