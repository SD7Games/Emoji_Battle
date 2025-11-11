using UnityEngine;

[DisallowMultipleComponent]
public class AISelector : MonoBehaviour
{
    [SerializeField] private AIComlexityLobbyUI _ui;

    private void Start()
    {
        if (_ui != null)
            _ui.OnDifficultyChanged += HandleDifficultyChange;

        string savedStrategy = GD.AI.Strategy;
        HandleDifficultyChange(savedStrategy);
    }

    private void OnDestroy()
    {
        if (_ui != null)
            _ui.OnDifficultyChanged -= HandleDifficultyChange;
    }

    private void HandleDifficultyChange(string difficulty)
    {
        GD.AI.Strategy = difficulty;
        GD.Save();
    }
}
