using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BootstrapView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _progressText;

    private int _currentProgress = 0;
    private Tween _progressTween;

    private void OnDisable()
    {
        _progressTween?.Kill();
    }

    private void OnDestroy()
    {
        _progressTween?.Kill();
    }

    public void SetProgress(int progress)
    {
        _progressTween?.Kill();

        if (_slider == null || _progressText == null)
            return;

        progress = Mathf.Clamp(progress, 0, 100);
        float duration = Mathf.Max(0.05f, Mathf.Abs(progress - _currentProgress) * 0.02f);

        _progressTween = DOTween.To(() => _currentProgress, x =>
        {
            if (_slider == null || _progressText == null)
            {
                _progressTween?.Kill();
                return;
            }

            _currentProgress = x;
            _slider.value = x / 100f;
            _progressText.text = $"{x}%";

        }, progress, duration)
        .SetEase(Ease.Linear)
        .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}
