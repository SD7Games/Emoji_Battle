using System.Collections.Generic;
using UnityEngine;

public class EmojiResolver
{
    private readonly Dictionary<int, EmojiData> _byColor = new();

    public EmojiResolver(List<EmojiData> sets)
    {
        foreach (var data in sets)
        {
            if (data == null) continue;
            _byColor[data.ColorId] = data;
        }
    }

    public Sprite Get(int color, int index)
    {
        if (!_byColor.TryGetValue(color, out var data))
            return null;

        if (data.EmojiSprites == null || data.EmojiSprites.Count == 0)
            return null;

        if (index < 0 || index >= data.EmojiSprites.Count)
            index = 0;

        return data.EmojiSprites[index];
    }

    public EmojiData GetData(int color)
    {
        _byColor.TryGetValue(color, out var data);
        return data;
    }
}