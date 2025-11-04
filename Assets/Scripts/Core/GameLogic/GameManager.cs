using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [Header("Board Elements")]
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private SettingsPopupView _uiView;
    [SerializeField] private WinLineView _boardView;
    [SerializeField] private AIRivalMoveController _aiRivalController;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _aiRivalName;
    [SerializeField] private Button _backToLobbyButton;

    [Header("Emoji Data (all colors)")]
    [SerializeField] private List<EmojiData> _emojiDataByColor;

    private Sprite _playerSprite;
    private Sprite _aiRivalSprite;
    private Sprite _defaultSprite;

    private InputController _input;
    private BoardController _board;
    private TurnManager _turnManager;
    private WinChecker _winChecker;

    private const string LobbyScene = "Lobby";

    private void Start()
    {
        SetSpriteReferences();
        UpdatePlayerNames();
        InitializeGameLogic();
        InitializeInput();

        _aiRivalController.Initialize(_input, _board);

        _uiView.OnRestartClicked += RestartGame;
        _backToLobbyButton.onClick.AddListener(LoadLobbyScene);

        RestartGame();
    }

    private void OnDestroy()
    {
        if (_input != null)
            _input.OnCellClicked -= OnCellClicked;

        if (_uiView != null)
            _uiView.OnRestartClicked -= RestartGame;
    }

    private void SetSpriteReferences()
    {
        _defaultSprite = _buttons[0].GetComponent<Image>().sprite;

        string playerColor = AISettingManager.Player.GetEmojiColor();
        int playerIndex = AISettingManager.Player.GetEmojiIndex();
        string aiColor = AISettingManager.AI.GetEmojiAIColor();
        int aiIndex = AISettingManager.AI.GetEmojiAIIndex();

        EmojiData playerData = _emojiDataByColor.Find(colorData => colorData.ColorName == playerColor);
        EmojiData aiData = _emojiDataByColor.Find(colorData => colorData.ColorName == aiColor);

        if (playerData != null && playerData.EmojiSprites.Count > 0)
            _playerSprite = playerData.GetEmojiByIndex(playerIndex);

        if (aiData != null && aiData.EmojiSprites.Count > 0)
            _aiRivalSprite = aiData.GetEmojiByIndex(aiIndex);
    }

    private void UpdatePlayerNames()
    {
        _playerName.text = AISettingManager.Player.GetName();
        _aiRivalName.text = "AI";
    }

    private void InitializeGameLogic()
    {
        _board = new BoardController(_buttons, _defaultSprite);
        _board.LoadEmojis(_emojiDataByColor);

        _turnManager = new TurnManager(_playerSprite, _aiRivalSprite, _playerName, _aiRivalName);
        _winChecker = new WinChecker();
    }

    private void InitializeInput()
    {
        _input = new InputController(_buttons);
        _input.OnCellClicked += OnCellClicked;
    }

    private void RestartGame()
    {
        _aiRivalController.StopAllCoroutines();
        _input.AllowInput();

        _board.Reset();
        _turnManager.Reset();
        _boardView.HideAllLines();

        _uiView.ShowCurrentPlayer(_turnManager.CurrentName());
    }

    private void OnCellClicked(int index)
    {
        if (!_board.IsCellEmpty(index)) return;

        CellState currentState = _turnManager.CurrentState();
        Sprite currentSprite = _turnManager.CurrentSprite();

        _board.SetCell(index, currentState, currentSprite);
        CellState[,] boardState = _board.GetBoardState();

        if (_winChecker.IsGameOver(boardState, out CellState winner, out WinLineView.WinLineType? winLine))
        {
            if (winner != CellState.Empty)
            {
                if (winLine.HasValue)
                    _boardView.ShowWinLine(winLine.Value);

                string winnerName = GetNameByState(winner);
                _uiView.ShowResult($"\n{winnerName} wins!");
            }
            else
            {
                _uiView.ShowResult("Draw!");
            }

            _board.DisableAll(boardState);
        }
        else
        {
            _turnManager.NextTurn();
            _uiView.ShowCurrentPlayer(_turnManager.CurrentName());

            if (_turnManager.CurrentState() == CellState.AI)
                _aiRivalController.MakeMove();
        }
    }

    private string GetNameByState(CellState state)
    {
        return state switch
        {
            CellState.Player => _playerName.text,
            CellState.AI => _aiRivalName.text,
            _ => "Unknown"
        };
    }

    public void LoadLobbyScene()
    {
        _aiRivalController.StopAllCoroutines();
        _input.AllowInput();
        SceneManager.LoadScene(LobbyScene);
    }
}
