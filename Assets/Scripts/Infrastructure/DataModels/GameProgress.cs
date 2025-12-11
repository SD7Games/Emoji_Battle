using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameProgress
{
    public List<EmojiProgress> All = new List<EmojiProgress>();

    [NonSerialized]
    private Dictionary<(int color, int id), EmojiProgress> _cache;

    public int Wins = 0;
    public int LootBoxes = 0;

    private void EnsureCache()
    {
        if (_cache != null)
            return;

        _cache = new Dictionary<(int color, int id), EmojiProgress>();

        foreach (var i in All)
        {
            var key = (i.ColorId, i.EmojiId);
            _cache[key] = i;
        }
    }

    private EmojiProgress GetOrCreate(int colorId, int emojiId)
    {
        EnsureCache();

        var key = (colorId, emojiId);

        if (_cache.TryGetValue(key, out var progress))
            return progress;

        progress = new EmojiProgress(colorId, emojiId, false);
        _cache[key] = progress;
        All.Add(progress);

        return progress;
    }

    public bool IsEmojiUnlocked(int colorId, int emojiId)
    {
        EnsureCache();

        if (_cache.TryGetValue((colorId, emojiId), out var p))
            return p.IsUnlocked;

        return false;
    }

    public void UnlockEmoji(int colorId, int emojiId)
    {
        var p = GetOrCreate(colorId, emojiId);

        if (p.IsUnlocked)
            return;

        p.IsUnlocked = true;
    }

    public void UnlockFirstNAllColors(Dictionary<int, int> totalPerColor, int n)
    {
        EnsureCache();

        foreach (var kvp in totalPerColor)
        {
            int color = kvp.Key;
            int count = Mathf.Min(n, kvp.Value);

            for (int i = 0; i < count; i++)
                UnlockEmoji(color, i);
        }
    }

    public bool UnlockNextGlobal(List<EmojiData> allColors)
    {
        EnsureCache();

        int max = 0;
        foreach (var e in allColors)
            max = Mathf.Max(max, e.EmojiSprites.Count);

        for (int i = 0; i < max; i++)
        {
            bool any = false;

            foreach (var c in allColors)
            {
                if (i >= c.EmojiSprites.Count)
                    continue;

                var key = (c.ColorId, i);

                if (!_cache.TryGetValue(key, out var p) || !p.IsUnlocked)
                {
                    UnlockEmoji(c.ColorId, i);
                    any = true;
                }
            }

            if (any)
                return true;
        }

        return false;
    }

    public bool UnlockRandomLocked(List<EmojiData> allColors)
    {
        EnsureCache();

        int max = 0;
        foreach (var e in allColors)
            max = Mathf.Max(max, e.EmojiSprites.Count);

        int chosenIndex = -1;
        int seen = 0;

        for (int i = 0; i < max; i++)
        {
            bool anyLocked = false;

            foreach (var c in allColors)
            {
                if (i >= c.EmojiSprites.Count)
                    continue;

                var key = (c.ColorId, i);

                if (!_cache.TryGetValue(key, out var p) || !p.IsUnlocked)
                {
                    anyLocked = true;
                    break;
                }
            }

            if (!anyLocked)
                continue;

            seen++;

            if (UnityEngine.Random.Range(0, seen) == 0)
                chosenIndex = i;
        }

        if (chosenIndex == -1)
            return false;

        foreach (var c in allColors)
        {
            if (chosenIndex < c.EmojiSprites.Count)
                UnlockEmoji(c.ColorId, chosenIndex);
        }

        return true;
    }

    public List<int> GetSortedEmojiIndexes(int colorId, int totalCount)
    {
        EnsureCache();

        int safeCount = Mathf.Max(0, totalCount);
        var result = new List<int>(safeCount);

        for (int i = 0; i < safeCount; i++)
        {
            var key = (colorId, i);

            if (_cache.TryGetValue(key, out var p) && p.IsUnlocked)
                result.Add(i);
        }

        for (int i = 0; i < safeCount; i++)
        {
            var key = (colorId, i);

            if (!_cache.TryGetValue(key, out var p) || !p.IsUnlocked)
                result.Add(i);
        }

        return result;
    }

    public bool HasAnyProgress()
    {
        return All.Count > 0;
    }
}