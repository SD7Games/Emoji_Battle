using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AIDifficultyDisplayUI : MonoBehaviour
{
    [SerializeField] private Image _background;

    private readonly Dictionary<string, Color> _difficultyColors = new()
    {
        { "Easy", Color.green },
        { "Norm", Color.yellow },
        { "Hard", Color.red }
    };

    private void Start()
    {
        string diff = GD.AI.Strategy;

        if (_difficultyColors.TryGetValue(diff, out var color))
        {
            color.a = 0.2f;
            _background.color = color;
        }
    }
}
