using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class EmojiIdleAnimation : MonoBehaviour
{
    [Header("Enable / Disable")]
    [SerializeField] private bool _enableBreathing = true;
    [SerializeField] private bool _enableTilting = true;

    [Header("Breathing / Scaling")]
    [SerializeField] private float _scaleAmplitude = 0.05f;
    [SerializeField] private float _scaleDuration = 1.6f;

    [Header("Tilting / Rotation")]
    [SerializeField] private float _tiltAngle = 8f;
    [SerializeField] private float _tiltDuration = 2.2f;

    private RectTransform _rect;
    private Vector3 _startScale;
    private Vector3 _startRotationEuler;
    private Tween _scaleTween;
    private Tween _tiltTween;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _startScale = transform.localScale;
        _startRotationEuler = _rect.localEulerAngles;
    }

    private void OnEnable()
    {
        ResetToStart();

        if (_enableBreathing)
            AnimateScale();

        if (_enableTilting)
            AnimateTilt();
    }

    private void OnDisable()
    {
        _scaleTween?.Kill();
        _tiltTween?.Kill();
    }

    private void ResetToStart()
    {
        transform.localScale = _startScale;
        _rect.localRotation = Quaternion.Euler(_startRotationEuler);
    }

    private void AnimateScale()
    {
        _scaleTween = transform.DOScale(
                _startScale * (1f + _scaleAmplitude),
                _scaleDuration
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(Random.Range(0f, 0.5f))
            .SetUpdate(true);
    }

    private void AnimateTilt()
    {
        float angle = _tiltAngle * (Random.value > 0.5f ? 1 : -1);
        Vector3 targetRotation = _startRotationEuler + new Vector3(0f, 0f, angle);

        _tiltTween = _rect.DOLocalRotate(
                targetRotation,
                _tiltDuration
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(Random.Range(0f, 0.5f))
            .SetUpdate(true);
    }
}
