using System.Collections.Generic;
using UnityEngine;

public sealed class MainInstaller : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] private MainUIView _uiView;

    [SerializeField] private MainSignView _signView;
    [SerializeField] private TurnState _turnState;
    [SerializeField] private AIMoveController _aiController;
    [SerializeField] private WinLineView _winLines;

    [Header("SFX")]
    [SerializeField] private SoundDefinition _playerMoveSfx;

    [SerializeField] private SoundDefinition _aiMoveSfx;
    [SerializeField] private SoundDefinition _backToLobbySfx;

    [Header("Popups")]
    [SerializeField] private PopupCanvasController _popupCanvas;

    [SerializeField] private PopupBase[] _scenePopups;

    [Header("Emoji Sets")]
    [SerializeField] private List<EmojiData> _emojiSets;

    private InputController _input;
    private EmojiResolver _resolver;
    private BoardState _board;
    private WinChecker _checker;
    private GameFlow _flow;
    private GameSession _session;

    private GameRewardService _rewardService;
    private GameResultController _resultController;

    private MainController _mainController;

    private void Awake()
    {
        _resolver = new EmojiResolver(_emojiSets);

        InitPopups();
        InjectPopupDependencies();

        InitGameFlow();

        _rewardService = new GameRewardService(_emojiSets);
        _resultController = new GameResultController(_winLines, _rewardService, this, _input);
        _session = new GameSession(_flow, _turnState, _winLines, _uiView.BoardView);

        _mainController = new MainController(
            _flow,
            _session,
            _board,
            _uiView,
            _signView,
            _aiController,
            _input,
            _resultController,
            _rewardService,
            _resolver,
            PopupService.I,
            AudioService.I,
            _scenePopups,
            _playerMoveSfx,
            _aiMoveSfx,
            _backToLobbySfx
        );

        _mainController.Initialize();
    }

    private void Start()
    {
        _mainController.PlayIntro();
    }

    private void OnDestroy()
    {
        _mainController?.Dispose();
    }

    private void InitPopups()
    {
        if (PopupService.I == null)
        {
            Debug.LogError("PopupService not initialized");
            return;
        }

        PopupService.I.SetContext(_popupCanvas, _scenePopups);
    }

    private void InjectPopupDependencies()
    {
        foreach (var popup in _scenePopups)
        {
            if (popup is IEmojiResolverConsumer consumer)
                consumer.Construct(_resolver);
        }
    }

    private void InitGameFlow()
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
#endif
        _board = new BoardState();
        _checker = new WinChecker();
        _flow = new GameFlow(_board, _turnState, _checker);

        _input = new InputController(_uiView.BoardView.Buttons);

        var save = GameDataService.I.Data;

        Sprite player = _resolver.Get(save.Player.EmojiColor, save.Player.EmojiIndex);
        Sprite ai = _resolver.Get(save.AI.EmojiColor, save.AI.EmojiIndex);

        _uiView.InitBoardSprites(player, ai);
        _uiView.BoardView.ResetView();
        _uiView.BoardView.SetInteractable(true);

        _flow.OnMoveApplied += _uiView.BoardView.OnMoveApplied;

        _aiController.Init(_flow);
    }
}