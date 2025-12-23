using System;
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
        var data = _resolver.GetData(colorId);
        if (data == null) return Array.Empty<EmojiViewData>();

        var progress = _data.Data.Progress;
        var sorted = progress.GetSortedForView(colorId, data.EmojiSprites.Count);

        var result = new EmojiViewData[sorted.Count];

        for (int i = 0; i < sorted.Count; i++)
        {
            var p = sorted[i];

            result[i] = new EmojiViewData
            {
                EmojiIndex = p.EmojiId,
                Sprite = data.EmojiSprites[p.EmojiId],
                Unlocked = p.IsUnlocked
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