using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class AIComplexityController : MonoBehaviour
{

    public event Action<AIStrategyType> OnDifficultyChanged;

    private AIComplexityView _view;
    private AIStrategyType _current;
    private bool _opened;
    private bool _initialized;

    public void Initialize(AIComplexityView view)
    {
        if (_initialized) return;
        _view = view;
        InternalInit();
    }

    private void Start()
    {
        if (_initialized) return;
        InternalInit();
    }

    private void InternalInit()
    {
        _current = AIComplexityService.Load();

        _view.SetMain(_current);
        _view.HideInstant();

        _view.OnMainClick += Toggle;
        _view.OnOptionClick += Select;

        _view.StartPulse();

        _opened = false;
        _initialized = true;
    }

    private void Toggle()
    {
        if (_opened)
        {
            _view.Collapse();
            _opened = false;
            return;
        }

        var other = GetOtherTwo(_current);

        _view.SetOption1(other[0]);
        _view.SetOption2(other[1]);

        _view.Expand();
        _opened = true;
    }

    private void Select(Button btn)
    {
        var label = btn.GetComponentInChildren<TMP_Text>().text;
        if (!Enum.TryParse(label, out AIStrategyType diff)) return;

        _current = diff;
        AIComplexityService.Save(diff);

        _view.SetMain(diff);
        _view.Collapse();

        _opened = false;
        OnDifficultyChanged?.Invoke(diff);
    }

    private List<AIStrategyType> GetOtherTwo(AIStrategyType current)
    {
        var all = new List<AIStrategyType>
        {
            AIStrategyType.Easy,
            AIStrategyType.Norm,
            AIStrategyType.Hard
        };

        all.Remove(current);
        return all;
    }
}