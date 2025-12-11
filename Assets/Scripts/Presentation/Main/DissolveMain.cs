using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DissolveMain : MonoBehaviour
{
    [SerializeField] private float _dissolveTime = 0.75f;

    private Image _image;
    private Material _material;
    private Coroutine _routine;

    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private const float StartValue = 1.1f;
    private const float EndValue = 0f;

    private bool _initialized = false;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (_initialized)
            ResetDissolve();
    }

    private void Initialize()
    {
        if (_initialized)
            return;

        _image = GetComponent<Image>();

        _material = Instantiate(_image.material);
        _image.material = _material;

        _initialized = true;
    }

    public void PlayDissolve()
    {
        if (!_initialized)
            Initialize();

        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(DissolveRoutine());
    }

    public void ResetDissolve()
    {
        if (!_initialized)
            Initialize();

        if (_routine != null)
            StopCoroutine(_routine);

        _material.SetFloat(DissolveAmount, StartValue);
    }

    private IEnumerator DissolveRoutine()
    {
        float elapsed = 0f;

        _material.SetFloat(DissolveAmount, StartValue);

        while (elapsed < _dissolveTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _dissolveTime;

            _material.SetFloat(DissolveAmount, Mathf.Lerp(StartValue, EndValue, t));

            yield return null;
        }

        _material.SetFloat(DissolveAmount, EndValue);
    }
}