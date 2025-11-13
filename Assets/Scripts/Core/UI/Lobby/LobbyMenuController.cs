using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class LobbyMenuController : MonoBehaviour
{
    private const string MainSceneTag = "Main";

    [Header("Start Reference")]
    [SerializeField] private Button _startButton;

    [Header("Settings References")]
    [SerializeField] private Button _settingsButton;

    private void Start()
    {
        _startButton.onClick.AddListener(LoadMainScene);
        _settingsButton.onClick.AddListener(OpenSettings);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(LoadMainScene);
        _settingsButton.onClick.RemoveListener(OpenSettings);
    }

    private void OpenSettings()
    {
        // TODO: open settings popup
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(MainSceneTag);
    }
}
