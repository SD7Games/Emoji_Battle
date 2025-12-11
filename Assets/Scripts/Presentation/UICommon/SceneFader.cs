using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField]
    private Image _sceneFaderImage;

    private float _fadeDuration = 1.5f;

    private void Start()
    {
        DoFade();
    }

    private void DoFade()
    {
        _sceneFaderImage.gameObject.SetActive(true);
        _sceneFaderImage.canvasRenderer.SetAlpha(1f);
        _sceneFaderImage.CrossFadeAlpha(0f, _fadeDuration, false);
    }
}