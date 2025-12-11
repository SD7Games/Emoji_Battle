using UnityEngine;
using UnityEngine.UI;
using System;

public class MainUIView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button restartButton;

    [SerializeField] private Button backButton;

    [Header("Board")]
    [SerializeField] private BoardView boardView;

    public BoardView BoardView => boardView;

    public event Action OnRestartClicked;

    public event Action OnBackClicked;

    public event Action<int> OnCellClicked;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
        backButton.onClick.AddListener(() => OnBackClicked?.Invoke());

        boardView.OnCellPressed += index => OnCellClicked?.Invoke(index);
    }

    public void InitBoardSprites(Sprite player, Sprite ai)
    {
        boardView.AssignSprites(player, ai);
    }
}