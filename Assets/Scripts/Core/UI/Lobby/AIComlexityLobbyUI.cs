using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AIComlexityLobbyUI : MonoBehaviour
{
    [SerializeField] private Button _mainButton;
    [SerializeField] private Button _opt1Button;
    [SerializeField] private Button _opt2Button;
    [SerializeField] private CanvasGroup _opt1CanvasGroup;
    [SerializeField] private CanvasGroup _opt2CanvasGroup;
    [SerializeField] private RectTransform _mainRT;
    [SerializeField] private RectTransform _opt1RT;
    [SerializeField] private RectTransform _opt2RT;

    private Vector2 _opt1Pos, _opt2Pos;
    private string _currentDifficulty;
    private bool _optionsVisible;
    private bool _canPulse = true;

    private Tween _pulseTween;
    private Sequence _showSequence;
    private Sequence _hideSequence;

    private readonly Dictionary<string, Color> _difficultyColors = new()
    {
        { "Easy", Color.green },
        { "Norm", Color.yellow },
        { "Hard", Color.red }
    };

    public event Action<string> OnDifficultyChanged;

    private void Start()
    {
        _opt1Pos = _opt1RT.anchoredPosition;
        _opt2Pos = _opt2RT.anchoredPosition;

        _currentDifficulty = GD.AI.Strategy;
        UpdateMainButton(_currentDifficulty);
        HideOptionsInstant();

        _mainButton.onClick.AddListener(ToggleOptions);
        _opt1Button.onClick.AddListener(() => OnSelect(_opt1Button));
        _opt2Button.onClick.AddListener(() => OnSelect(_opt2Button));

        StartPulse();
    }

    private void OnDisable() => StopAllTweens();

    private void OnDestroy() => StopAllTweens();

    private void ToggleOptions()
    {
        if (_optionsVisible)
            HideOptionsAnimated();
        else
            ShowOptionsAnimated();
    }

    private void OnSelect(Button selectedButton)
    {
        string newDiff = selectedButton.GetComponentInChildren<TMP_Text>().text;
        _currentDifficulty = newDiff;

        GD.AI.Strategy = _currentDifficulty;
        GD.Save();

        UpdateMainButton(_currentDifficulty);
        HideOptionsAnimated();

        OnDifficultyChanged?.Invoke(_currentDifficulty);
    }

    private void ShowOptionsAnimated()
    {
        _canPulse = false;
        StopPulse();
        _optionsVisible = true;

        List<string> otherOptions = new(_difficultyColors.Keys);
        otherOptions.Remove(_currentDifficulty);
        SetupOption(_opt1Button, otherOptions[0]);
        SetupOption(_opt2Button, otherOptions[1]);

        _opt1CanvasGroup.interactable = true;
        _opt2CanvasGroup.interactable = true;
        _opt1CanvasGroup.blocksRaycasts = true;
        _opt2CanvasGroup.blocksRaycasts = true;

        _showSequence?.Kill();
        _hideSequence?.Kill();

        _opt1CanvasGroup.alpha = 0f;
        _opt2CanvasGroup.alpha = 0f;

        _opt1RT.anchoredPosition = _opt1Pos - new Vector2(0, 20);
        _opt2RT.anchoredPosition = _opt2Pos - new Vector2(0, 20);

        _showSequence = DOTween.Sequence()
            .Append(_opt1CanvasGroup.DOFade(1f, 0.25f))
            .Join(_opt1RT.DOAnchorPos(_opt1Pos, 0.25f).SetEase(Ease.OutQuad))
            .Join(_opt2CanvasGroup.DOFade(1f, 0.3f))
            .Join(_opt2RT.DOAnchorPos(_opt2Pos, 0.3f).SetEase(Ease.OutQuad))
            .SetLink(gameObject);
    }

    private void HideOptionsAnimated()
    {
        _optionsVisible = false;

        _showSequence?.Kill();
        _hideSequence?.Kill();

        _hideSequence = DOTween.Sequence()
            .Append(_opt1CanvasGroup.DOFade(0f, 0.25f))
            .Join(_opt2CanvasGroup.DOFade(0f, 0.25f))
            .Join(_opt1RT.DOAnchorPos(_opt1Pos - new Vector2(0, 20), 0.25f))
            .Join(_opt2RT.DOAnchorPos(_opt2Pos - new Vector2(0, 20), 0.25f))
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                _opt1CanvasGroup.interactable = false;
                _opt2CanvasGroup.interactable = false;
                _opt1CanvasGroup.blocksRaycasts = false;
                _opt2CanvasGroup.blocksRaycasts = false;
                _canPulse = true;
            })
            .SetLink(gameObject);
    }

    private void HideOptionsInstant()
    {
        _opt1CanvasGroup.alpha = 0f;
        _opt2CanvasGroup.alpha = 0f;
        _opt1CanvasGroup.interactable = false;
        _opt2CanvasGroup.interactable = false;
        _opt1CanvasGroup.blocksRaycasts = false;
        _opt2CanvasGroup.blocksRaycasts = false;
        _opt1RT.anchoredPosition = _opt1Pos;
        _opt2RT.anchoredPosition = _opt2Pos;
        _optionsVisible = false;
        _canPulse = true;
    }

    private void SetupOption(Button button, string diff)
    {
        button.GetComponentInChildren<TMP_Text>().text = diff;
        button.GetComponent<Image>().color = _difficultyColors[diff];
    }

    private void UpdateMainButton(string diff)
    {
        _mainButton.GetComponentInChildren<TMP_Text>().text = diff;
        _mainButton.GetComponent<Image>().color = _difficultyColors[diff];
    }

    private void StartPulse()
    {
        if (!_canPulse || _mainRT == null)
            return;

        StopPulse();

        _pulseTween = _mainRT.DOScale(1.2f, 0.25f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(4f)
            .SetLink(gameObject);
    }

    private void StopPulse()
    {
        if (_pulseTween != null && _pulseTween.IsActive())
        {
            _pulseTween.Kill(false);
            _pulseTween = null;
            if (_mainRT != null)
                _mainRT.localScale = Vector3.one;
        }
    }

    private void StopAllTweens()
    {
        StopPulse();
        _showSequence?.Kill();
        _hideSequence?.Kill();
    }
}
