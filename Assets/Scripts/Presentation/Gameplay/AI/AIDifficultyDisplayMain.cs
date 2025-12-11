using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIDifficultyDisplayUI : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _text;

    private readonly Dictionary<AIStrategyType, Color> _difficultyColors = new()
    {
        { AIStrategyType.Easy,  new Color(0.00f, 0.96f, 1.00f, 0.55f) }, // #00F5FF
        { AIStrategyType.Norm,  new Color(0.78f, 0.47f, 1.00f, 0.55f) }, // #C778FF
        { AIStrategyType.Hard,  new Color(1.00f, 0.18f, 0.18f, 0.55f) }, // #FF2E2E
    };

    private readonly Dictionary<AIStrategyType, string> _difficultyNames = new()
    {
        { AIStrategyType.Easy,  "Easy" },
        { AIStrategyType.Norm,  "Normal" },
        { AIStrategyType.Hard,  "Hard" }
    };

    private void Start()
    {
        AIStrategyType diff = GameDataService.I.Data.AI.Strategy;

        if (_difficultyColors.TryGetValue(diff, out var color))
            _background.color = color;

        if (_difficultyNames.TryGetValue(diff, out var text))
            _text.text = text;
    }
}