using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    private const string MainSceneTag = "Main";

    [SerializeField]
    private Button _startButton;   

    private void Start()
    {
        _startButton.onClick.AddListener(() => LoadMainScene());
    }   

    private void LoadMainScene()
    {
        SceneManager.LoadScene(MainSceneTag);
    }
}
