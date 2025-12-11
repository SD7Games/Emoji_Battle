using System.Collections.Generic;
using UnityEngine;

public class LobbyInstaller : MonoBehaviour
{
    [SerializeField] private LobbyView _view;
    [SerializeField] private AIComplexityView _aiComplexityView;
    [SerializeField] private List<EmojiData> _emojiSets;

    private LobbyController _controller;
    private AIComplexityController _aiComplexityController;

    private void Start()
    {
        var dataService = GameDataService.I;
        var data = dataService.Data;

        if (!data.Progress.HasAnyProgress())
        {
            int firstN = 4;
            Dictionary<int, int> total = new();

            foreach (var set in _emojiSets)
            {
                if (set == null) continue;
                int count = set.EmojiSprites != null ? set.EmojiSprites.Count : 0;
                total[set.ColorId] = count;
            }

            data.Progress.UnlockFirstNAllColors(total, firstN);
            dataService.Save();
        }

        int savedColor = data.Player.EmojiColor;

        var resolver = new EmojiResolver(_emojiSets);
        var emojiService = new EmojiSelectionService(dataService, resolver);
        var aiService = new AISelectionService(dataService, _emojiSets);
        var lobbyService = new LobbyService(emojiService, aiService);

        _controller = new LobbyController(lobbyService);

        _aiComplexityController = _aiComplexityView.gameObject.AddComponent<AIComplexityController>();
        _aiComplexityController.Initialize(_aiComplexityView);

        _aiComplexityController.OnDifficultyChanged += _controller.OnAIStrategyChanged;

        _view.Construct(_controller);

        _controller.SetInitialColor(savedColor);

        Sprite savedPlayer = resolver.Get(data.Player.EmojiColor, data.Player.EmojiIndex);
        if (savedPlayer != null)
            _view.ForceSetPlayerAvatar(savedPlayer);
    }

    private void OnDestroy()
    {
        if (_aiComplexityController != null)
            _aiComplexityController.OnDifficultyChanged -= _controller.OnAIStrategyChanged;
    }
}