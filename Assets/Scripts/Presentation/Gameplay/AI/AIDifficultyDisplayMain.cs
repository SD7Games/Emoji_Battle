using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AIDifficultyDisplayUI : MonoBehaviour
{
    [SerializeField] private Image _background;

    private readonly Dictionary<AIStrategyType, Color> _difficultyColors = new()
    {
        { AIStrategyType.Easy,   Color.green },
        { AIStrategyType.Norm, Color.yellow },
        { AIStrategyType.Hard,   Color.red }
    };

    private void Start()
    {
        AIStrategyType diff = GameDataService.I.Data.AI.Strategy;

        if (_difficultyColors.TryGetValue(diff, out var color))
        {
            color.a = 0.8f;
            _background.color = color;
        }
    }
}