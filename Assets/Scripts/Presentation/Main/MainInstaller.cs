using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainInstaller : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] private MainUIView _uiView;

    [SerializeField] private MainSignView _signView;
    [SerializeField] private TurnState _turnState;
    [SerializeField] private AIMoveController _aiController;
    [SerializeField] private WinLineView _winLines;

    [Header("Emoji Sets")]
    [SerializeField] private List<EmojiData> _emojiSets;

    private InputController _input;
    private EmojiResolver _resolver;
    private BoardState _board;
    private WinChecker _checker;
    private GameFlow _flow;
    private GameSession _session;

    private GameResultUI _resultUI;
    private GameRewardService _rewardService;

    private void Awake()
    {
        var save = GameDataService.I.Data;

        _resolver = new EmojiResolver(_emojiSets);

        Sprite player = _resolver.Get(save.Player.EmojiColor, save.Player.EmojiIndex);
        Sprite ai = _resolver.Get(save.AI.EmojiColor, save.AI.EmojiIndex);

        _signView.SetPlayer(player, save.Player.Name);
        _signView.SetAI(ai, save.AI.Name);

        _board = new BoardState();
        _checker = new WinChecker();

        _flow = new GameFlow(_board, _turnState, _checker);

        _input = new InputController(_uiView.BoardView.Buttons);
        _input.OnCellClicked += index =>
        {
            _flow.ProcessMove(index);
        };

        _uiView.InitBoardSprites(player, ai);
        _uiView.BoardView.ResetView();
        _uiView.BoardView.SetInteractable(true);

        _flow.OnMoveApplied += _uiView.BoardView.OnMoveApplied;

        _aiController.Init(_flow);

        _flow.OnTurnChanged += isPlayerTurn =>
        {
            _uiView.BoardView.SetInteractable(isPlayerTurn);

            if (!isPlayerTurn)
                _aiController.MakeMove(_board.AsIntArray());
        };

        _resultUI = new GameResultUI(_winLines, _uiView.BoardView);
        _rewardService = new GameRewardService(_emojiSets);

        _flow.OnGameOver += (winner, line, finalBoard) =>
        {
            _resultUI.Show(winner, line, finalBoard);
            _rewardService.OnWin(winner);
            _uiView.BoardView.DisableAfterGameOver(finalBoard);
        };

        _session = new GameSession(_flow, _turnState, _winLines, _uiView.BoardView);
        _uiView.OnRestartClicked += _session.Restart;

        _uiView.OnBackClicked += () => SceneManager.LoadScene("Lobby");
    }

    private void Start()
    {
        _signView.PlayIntroDissolve();
    }
}