using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainInstaller : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] private MainUIView uiView;

    [SerializeField] private MainSignView signView;
    [SerializeField] private TurnState turnState;
    [SerializeField] private AIMoveController aiController;
    [SerializeField] private WinLineView winLines;

    [Header("Emoji Sets")]
    [SerializeField] private List<EmojiData> emojiSets;

    private InputController input;
    private EmojiResolver resolver;
    private BoardState board;
    private WinChecker checker;
    private GameFlow flow;
    private GameSession session;

    private GameResultUI resultUI;
    private GameRewardService rewardService;

    private void Awake()
    {
        var save = GameDataService.I.Data;

        resolver = new EmojiResolver(emojiSets);

        Sprite player = resolver.Get(save.Player.EmojiColor, save.Player.EmojiIndex);
        Sprite ai = resolver.Get(save.AI.EmojiColor, save.AI.EmojiIndex);

        signView.SetPlayer(player, save.Player.Name);
        signView.SetAI(ai, save.AI.Name);

        board = new BoardState();
        checker = new WinChecker();

        flow = new GameFlow(board, turnState, checker);

        input = new InputController(uiView.BoardView.Buttons);
        input.OnCellClicked += index =>
        {
            flow.ProcessMove(index);
        };

        uiView.InitBoardSprites(player, ai);
        uiView.BoardView.ResetView();
        uiView.BoardView.SetInteractable(true);

        flow.OnMoveApplied += uiView.BoardView.OnMoveApplied;

        aiController.Init(flow);

        flow.OnTurnChanged += isPlayerTurn =>
        {
            uiView.BoardView.SetInteractable(isPlayerTurn);

            if (!isPlayerTurn)
                aiController.MakeMove(board.AsIntArray());
        };

        resultUI = new GameResultUI(winLines, uiView.BoardView);
        rewardService = new GameRewardService(emojiSets);

        flow.OnGameOver += (winner, line, finalBoard) =>
        {
            resultUI.Show(winner, line, finalBoard);
            rewardService.OnWin(winner);
            uiView.BoardView.DisableAfterGameOver(finalBoard);
        };

        session = new GameSession(flow, turnState, winLines, uiView.BoardView);
        uiView.OnRestartClicked += session.Restart;

        uiView.OnBackClicked += () => SceneManager.LoadScene("Lobby");
    }

    private void Start()
    {
        signView.PlayIntroDissolve();
    }
}