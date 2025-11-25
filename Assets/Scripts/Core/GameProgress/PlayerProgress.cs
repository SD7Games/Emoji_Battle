using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerProgress
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

        foreach (var p in All)
        {
            var key = (p.ColorId, p.EmojiId);
            if (!_cache.ContainsKey(key))
                _cache[key] = p;
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
        p.IsUnlocked = true;
    }

    public void UnlockFirstNAllColors(Dictionary<int, int> totalPerColor, int n)
    {
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
        foreach (var c in allColors)
            max = Mathf.Max(max, c.EmojiSprites.Count);

        for (int i = 0; i < max; i++)
        {
            bool any = false;

            foreach (var c in allColors)
            {
                if (i >= c.EmojiSprites.Count)
                    continue;

                if (!IsEmojiUnlocked(c.ColorId, i))
                {
                    UnlockEmoji(c.ColorId, i);
                    any = true;
                }
            }

            if (any) return true;
        }

        return false;
    }
   
    public bool UnlockNextLocked(int colorId, int totalCount)
    {
        EnsureCache();

        for (int i = 0; i < totalCount; i++)
        {
            if (!IsEmojiUnlocked(colorId, i))
            {
                UnlockEmoji(colorId, i);
                return true;
            }
        }

        return false;
    }
   
    public bool UnlockRandomLocked(List<EmojiData> allColors)
    {
        EnsureCache();

        List<(int color, int index)> locked = new();

        foreach (var c in allColors)
        {
            for (int i = 0; i < c.EmojiSprites.Count; i++)
            {
                if (!IsEmojiUnlocked(c.ColorId, i))
                    locked.Add((c.ColorId, i));
            }
        }

        if (locked.Count == 0)
            return false;

        var choice = locked[UnityEngine.Random.Range(0, locked.Count)];
        UnlockEmoji(choice.color, choice.index);

        return true;
    }
  
    public List<int> GetSortedEmojiIndexes(int colorId, int totalCount)
    {
        EnsureCache();

        var result = new List<int>(totalCount);

        for (int i = 0; i < totalCount; i++)
            if (IsEmojiUnlocked(colorId, i))
                result.Add(i);

        for (int i = 0; i < totalCount; i++)
            if (!IsEmojiUnlocked(colorId, i))
                result.Add(i);

        return result;
    }
   
    public bool HasAnyProgress()
    {
        return All.Count > 0;
    }
}
