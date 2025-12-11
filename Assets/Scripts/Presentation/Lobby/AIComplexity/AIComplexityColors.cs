using System.Collections.Generic;
using UnityEngine;

public static class AIComplexityColors
{
    private static readonly Dictionary<AIStrategyType, Color> _colors = new()
    {
        { AIStrategyType.Easy, Color.green },
        { AIStrategyType.Norm, Color.yellow },
        { AIStrategyType.Hard, Color.red }
    };

    public static Color Get(AIStrategyType type) => _colors[type];
}