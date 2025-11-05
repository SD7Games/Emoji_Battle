using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenuController : MonoBehaviour
{
    private const string MainSceneTag = "Main";

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _modeButton;
    [SerializeField] private Button _settingsButton;

    private void Start()
    {
        _startButton.onClick.AddListener(LoadMainScene);
        _modeButton.onClick.AddListener(OpenModeSelection);
        _settingsButton.onClick.AddListener(OpenSettings);
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(MainSceneTag);
    }

    private void OpenModeSelection()
    {
        // TODO: открыть popup выбора режима
    }

    private void OpenSettings()
    {
        // TODO: открыть popup настроек
    }
}
