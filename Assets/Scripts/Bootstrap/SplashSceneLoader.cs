using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashSceneLoader : MonoBehaviour
{
    [SerializeField]
    private float _delayBeforeLoad = 2f;

    private const string LOAD_BOOTSTRAP = "Bootstrap";

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_delayBeforeLoad);
        LoaNextScene();

    }

    private void LoaNextScene()
    {
        SceneManager.LoadScene(LOAD_BOOTSTRAP);
    }
}
