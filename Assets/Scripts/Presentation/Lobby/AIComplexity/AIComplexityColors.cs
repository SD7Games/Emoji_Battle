using System.Collections.Generic;
using UnityEngine;

public static class AIComplexityColors
{
    private static readonly Dictionary<AIStrategyType, Color> _colors = new()
    {
        { AIStrategyType.Easy,  new Color(0.00f, 0.96f, 1.00f, 0.55f) }, // #00F5FF
        { AIStrategyType.Norm,  new Color(0.78f, 0.47f, 1.00f, 0.55f) }, // #C778FF
        { AIStrategyType.Hard,  new Color(1.00f, 0.18f, 0.18f, 0.55f) }  // #FF2E2E
    };

    public static Color Get(AIStrategyType type) => _colors[type];
}