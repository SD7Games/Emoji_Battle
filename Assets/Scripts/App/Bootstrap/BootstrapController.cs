using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapController
{
    private const string LobbySceneTag = "Lobby";

    private readonly BootstrapView _bootstrapView;

    private AsyncOperation _loadOperation;

    public BootstrapController(BootstrapView bootstrapView)
    {
        _bootstrapView = bootstrapView;
        _bootstrapView.SetProgress(0);
    }

    public async Task StartAsynk()
    {
        _loadOperation = SceneManager.LoadSceneAsync(LobbySceneTag);
        _loadOperation.allowSceneActivation = false;

        await Task.Delay(1000);
        _bootstrapView.SetProgress(11);

        await Task.Delay(1000);
        _bootstrapView.SetProgress(28);

        await Task.Delay(1000);
        _bootstrapView.SetProgress(63);

        await Task.Delay(1000);
        _bootstrapView.SetProgress(85);

        await Task.Delay(1000);
        _bootstrapView.SetProgress(100);

        await Task.Delay(300);

        _loadOperation.allowSceneActivation = true;
    }
}