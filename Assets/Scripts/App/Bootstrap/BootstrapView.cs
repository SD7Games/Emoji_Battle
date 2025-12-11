using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class BootstrapView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _progressText;

    private float _currentProgress;
    private Tween _progressTween;

    private void OnDisable()
    {
        _progressTween?.Kill();
        _progressTween = null;
    }

    public void SetProgress(int progress)
    {
        if (_slider == null || _progressText == null)
        {
            Debug.LogError("BootstrapView: UI references are missing");
            return;
        }

        _progressTween?.Kill();
        _progressTween = null;

        progress = Mathf.Clamp(progress, 0, 100);

        float duration = Mathf.Max(
            0.05f,
            Mathf.Abs(progress - _currentProgress) * 0.02f
        );

        _progressTween = DOTween.To(() => _currentProgress, x =>
            {
                _currentProgress = x;
                _slider.value = x / 100f;
                _progressText.text = $"{Mathf.RoundToInt(x)}%";
            },
            progress,
            duration
        )
        .SetEase(Ease.Linear);
    }
}