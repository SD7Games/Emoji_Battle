using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EmojiSelectionService
{
    private readonly GameDataService _data;
    private readonly EmojiResolver _resolver;

    public EmojiSelectionService(GameDataService data, EmojiResolver resolver)
    {
        _data = data;
        _resolver = resolver;
    }

    public EmojiViewData[] BuildEmojiList(int colorId)
    {
        EmojiData emojiData = _resolver.GetData(colorId);

        if (emojiData == null || emojiData.EmojiSprites == null)
            return Array.Empty<EmojiViewData>();

        GameProgress progress = _data.Data.Progress;

        int total = emojiData.EmojiSprites.Count;
        List<int> sorted = progress.GetSortedEmojiIndexes(colorId, total);

        var result = new EmojiViewData[sorted.Count];

        for (int i = 0; i < sorted.Count; i++)
        {
            int emojiIndex = sorted[i];

            result[i] = new EmojiViewData
            {
                EmojiIndex = emojiIndex,
                Sprite = emojiData.EmojiSprites[emojiIndex],
                Unlocked = progress.IsEmojiUnlocked(colorId, emojiIndex)
            };
        }

        return result;
    }

    public Sprite SelectPlayerEmoji(int colorId, int emojiIndex)
    {
        EmojiData emojiData = _resolver.GetData(colorId);
        if (emojiData == null || emojiData.EmojiSprites == null)
            return null;

        if (emojiIndex < 0 || emojiIndex >= emojiData.EmojiSprites.Count)
            return null;

        PlayerProfile player = _data.Data.Player;
        player.EmojiColor = colorId;
        player.EmojiIndex = emojiIndex;

        _data.Save();

        return emojiData.EmojiSprites[emojiIndex];
    }
}