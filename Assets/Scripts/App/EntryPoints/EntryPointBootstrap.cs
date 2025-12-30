using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class EntryPointBootstrap : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private BootstrapView _bootstrapView;

    [Header("Service Prefabs")]
    [SerializeField] private PopupService _popupServicePrefab;

    [SerializeField] private AudioService _audioServicePrefab;

    [Header("Audio")]
    [SerializeField] private MusicDefinition _lobbyMusic;

    [SerializeField] private float _musicFadeIn = 0.5f;

    private BootstrapController _controller;

    private void Awake()
    {
        EnsureServices();
        SceneManager.sceneLoaded += OnSceneLoaded;

        _controller = new BootstrapController(_bootstrapView);
    }

    private void Start()
    {
        _ = RunAsync();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private async Task RunAsync()
    {
        try
        {
            await _controller.StartAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Lobby")
            return;

        StartLobbyMusic();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void EnsureServices()
    {
        if (PopupService.I == null)
            Instantiate(_popupServicePrefab);

        if (AudioService.I == null)
            Instantiate(_audioServicePrefab);
    }

    private void StartLobbyMusic()
    {
        if (_lobbyMusic == null)
            return;

        AudioService.I.FadeToMusic(_lobbyMusic, _musicFadeIn);
    }
}