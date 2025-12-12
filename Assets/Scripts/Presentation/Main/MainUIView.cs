using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUIView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _restartButton;

    [SerializeField] private Button _backButton;

    [Header("Board")]
    [SerializeField] private BoardView _boardView;

    public BoardView BoardView => _boardView;

    public event Action OnRestartClicked;

    public event Action OnBackClicked;

    public event Action<int> OnCellClicked;

    private void Awake()
    {
        _restartButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
        _backButton.onClick.AddListener(() => OnBackClicked?.Invoke());

        _boardView.OnCellPressed += index => OnCellClicked?.Invoke(index);
    }

    public void InitBoardSprites(Sprite player, Sprite ai)
    {
        _boardView.AssignSprites(player, ai);
    }
}