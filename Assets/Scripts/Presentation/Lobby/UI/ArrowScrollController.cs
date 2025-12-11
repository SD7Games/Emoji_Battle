using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArrowScrollController : MonoBehaviour
{
    [Header("Scroll Components")]
    [SerializeField] private ScrollRect _scrollRect;

    [Header("Buttons")]
    [SerializeField] private Button _upButton;

    [SerializeField] private Button _downButton;

    [Header("Settings")]
    [SerializeField, Range(0.05f, 1f)]
    private float _step = 0.2f;

    [SerializeField, Range(0.05f, 1.5f)]
    private float _smoothTime = 0.25f;

    private Coroutine _scrollRoutine;
    private bool _isAnimating = false;

    private void Start()
    {
        _upButton.onClick.AddListener(() => Scroll(true));
        _downButton.onClick.AddListener(() => Scroll(false));

        _scrollRect.onValueChanged.AddListener(OnScrollChanged);

        UpdateArrowStates();
    }

    private void Scroll(bool up)
    {
        if (_isAnimating)
            return;

        float target = _scrollRect.verticalNormalizedPosition + (up ? _step : -_step);
        target = Mathf.Clamp01(target);

        if (_scrollRoutine != null)
            StopCoroutine(_scrollRoutine);

        _scrollRoutine = StartCoroutine(SmoothScroll(target));
    }

    private IEnumerator SmoothScroll(float target)
    {
        _isAnimating = true;
        SetRaycast(false);

        float start = _scrollRect.verticalNormalizedPosition;
        float time = 0f;

        while (time < _smoothTime)
        {
            time += Time.deltaTime;

            float t = time / _smoothTime;
            t = t * t * (3 - 2 * t);

            _scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, target, t);
            yield return null;
        }

        _scrollRect.verticalNormalizedPosition = target;

        SetRaycast(true);
        _isAnimating = false;

        UpdateArrowStates();
    }

    private void OnScrollChanged(Vector2 _)
    {
        if (!_isAnimating)
            UpdateArrowStates();
    }

    private void UpdateArrowStates()
    {
        float pos = _scrollRect.verticalNormalizedPosition;

        _upButton.interactable = pos < 0.98f;

        _downButton.interactable = pos > 0.02f;
    }

    private void SetRaycast(bool value)
    {
        _upButton.image.raycastTarget = value;
        _downButton.image.raycastTarget = value;
    }
}