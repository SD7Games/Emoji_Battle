using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class LobbyMenuController : MonoBehaviour
{
    private const string MainSceneTag = "Main";

    [Header("Start Reference")]
    [SerializeField] private Button _startButton;

    [Header("Mode References")]
    [SerializeField] private Button _modeButton;
    [SerializeField] private Button _playerVS_AI;
    [SerializeField] private Button _playerVS_Player;
    [SerializeField] private Button _closePopupBackgroundButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject _modePopup;

    [Header("Settings References")]
    [SerializeField] private Button _settingsButton;

    private void Start()
    {
        _startButton.onClick.AddListener(LoadMainScene);
        _modeButton.onClick.AddListener(OpenModePopup);
        _settingsButton.onClick.AddListener(OpenSettings);
        _playerVS_AI.onClick.AddListener(OnPlayerVsAI);
        _playerVS_Player.onClick.AddListener(OnPlayerVsPlayer);
        _closePopupBackgroundButton.onClick.AddListener(CloseModePopup);
        _closeButton.onClick.AddListener(CloseModePopup);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(LoadMainScene);
        _modeButton.onClick.RemoveListener(OpenModePopup);
        _settingsButton.onClick.RemoveListener(OpenSettings);
        _playerVS_AI.onClick.RemoveListener(OnPlayerVsAI);
        _playerVS_Player.onClick.RemoveListener(OnPlayerVsPlayer);
        _closePopupBackgroundButton.onClick.RemoveListener(CloseModePopup);
        _closeButton.onClick.RemoveListener(CloseModePopup);
    }

    private void OnPlayerVsAI() => SetGameMode(GameMode.PvE);
    private void OnPlayerVsPlayer() => SetGameMode(GameMode.PvP);

    private void OpenModePopup() => _modePopup.gameObject.SetActive(true);

    private void SetGameMode(GameMode mode)
    {
        GD.Mode = mode;
        GD.Save();

        CloseModePopup();
    }

    private void CloseModePopup() => _modePopup.gameObject.SetActive(false);

    private void OpenSettings()
    {
        // TODO: open to settings popup
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(MainSceneTag);
    }
}
