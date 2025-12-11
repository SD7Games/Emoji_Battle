using System.Collections.Generic;
using UnityEngine;

public sealed class AISelectionService
{
    private readonly GameDataService _data;
    private readonly List<EmojiData> _emojiSets;

    public AISelectionService(GameDataService data, List<EmojiData> emojiSets)
    {
        _data = data;
        _emojiSets = emojiSets;
    }

    public string GenerateRandomName()
    {
        var ai = _data.Data.AI;
        string newName = AINameProvider.GetRandomName(ai.Strategy);
        ai.Name = newName;
        _data.Save();
        return newName;
    }

    public string GetCurrentName() => _data.Data.AI.Name;

    public Sprite EnsureValidAIEmoji()
    {
        var save = _data.Data;
        var player = save.Player;
        var ai = save.AI;

        Sprite current = TryGetSprite(ai.EmojiColor, ai.EmojiIndex);

        bool isValid =
            current != null &&
            ai.EmojiColor != player.EmojiColor &&
            ai.EmojiIndex != player.EmojiIndex;

        return isValid
            ? current
            : PickEmojiAvoidingPlayer();
    }

    public Sprite PickEmojiAvoidingPlayer()
    {
        var save = _data.Data;
        var player = save.Player;

        int forbiddenColor = player.EmojiColor;
        int forbiddenIndex = player.EmojiIndex;

        List<(int color, int index, Sprite sprite)> candidates = new();

        foreach (var set in _emojiSets)
        {
            if (set == null || set.EmojiSprites == null) continue;

            for (int i = 0; i < set.EmojiSprites.Count; i++)
            {
                Sprite sprite = set.EmojiSprites[i];
                if (sprite == null) continue;

                if (set.ColorId == forbiddenColor) continue;

                if (i == forbiddenIndex) continue;

                candidates.Add((set.ColorId, i, sprite));
            }
        }

        if (candidates.Count == 0)
        {
            foreach (var set in _emojiSets)
            {
                if (set == null || set.EmojiSprites == null) continue;

                for (int i = 0; i < set.EmojiSprites.Count; i++)
                {
                    Sprite sprite = set.EmojiSprites[i];
                    if (sprite == null) continue;

                    candidates.Add((set.ColorId, i, sprite));
                }
            }

            if (candidates.Count == 0)
                return null;
        }

        var choice = candidates[Random.Range(0, candidates.Count)];

        save.AI.EmojiColor = choice.color;
        save.AI.EmojiIndex = choice.index;

        _data.Save();

        return choice.sprite;
    }

    private Sprite TryGetSprite(int colorId, int index)
    {
        foreach (var set in _emojiSets)
        {
            if (set.ColorId != colorId) continue;
            if (index < 0 || index >= set.EmojiSprites.Count) return null;

            return set.EmojiSprites[index];
        }

        return null;
    }
}