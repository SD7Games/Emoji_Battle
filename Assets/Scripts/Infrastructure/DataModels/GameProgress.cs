using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameProgress
{
    public List<EmojiProgress> AllEmoji = new();

    [NonSerialized]
    private Dictionary<(int, int), EmojiProgress> _cache;

    private int _unlockCounter = 0;

    public int LastUnlockedGlobalIndex { get; private set; } = -1;

    public bool HasAnyProgress() => AllEmoji.Count > 0;

    public bool IsEmojiUnlocked(int colorId, int emojiId)
    {
        EnsureCache();
        return _cache.TryGetValue((colorId, emojiId), out var p) && p.IsUnlocked;
    }

    public void UnlockEmoji(int colorId, int emojiId)
    {
        var p = GetOrCreate(colorId, emojiId);
        if (p.IsUnlocked) return;

        p.IsUnlocked = true;
        p.UnlockOrder = _unlockCounter++;
    }

    public void UnlockFirstNAllColors(
    Dictionary<int, int> totalPerColor,
    int n
)
    {
        EnsureCache();

        foreach (var kvp in totalPerColor)
        {
            int colorId = kvp.Key;
            int total = kvp.Value;

            int count = Mathf.Min(n, total);

            for (int i = 0; i < count; i++)
            {
                var progress = GetOrCreate(colorId, i);

                if (progress.IsUnlocked)
                    continue;

                progress.IsUnlocked = true;
                progress.UnlockOrder = _unlockCounter++;
            }
        }
    }

    public bool UnlockNextGlobal(List<EmojiData> sets)
    {
        EnsureCache();

        int max = 0;
        foreach (var s in sets)
            max = Mathf.Max(max, s.EmojiSprites.Count);

        for (int i = 0; i < max; i++)
        {
            bool unlockedAny = false;

            foreach (var set in sets)
            {
                if (i >= set.EmojiSprites.Count) continue;

                if (!IsEmojiUnlocked(set.ColorId, i))
                {
                    UnlockEmoji(set.ColorId, i);
                    unlockedAny = true;
                }
            }

            if (unlockedAny)
            {
                LastUnlockedGlobalIndex = i;
                return true;
            }
        }

        return false;
    }

    public bool UnlockRandomLocked(List<EmojiData> sets)
    {
        EnsureCache();

        var locked = new List<int>();

        int max = 0;
        foreach (var s in sets)
            max = Mathf.Max(max, s.EmojiSprites.Count);

        for (int i = 0; i < max; i++)
        {
            foreach (var s in sets)
            {
                if (i < s.EmojiSprites.Count && !IsEmojiUnlocked(s.ColorId, i))
                {
                    locked.Add(i);
                    break;
                }
            }
        }

        if (locked.Count == 0)
            return false;

        int chosen = locked[UnityEngine.Random.Range(0, locked.Count)];

        foreach (var s in sets)
            if (chosen < s.EmojiSprites.Count)
                UnlockEmoji(s.ColorId, chosen);

        LastUnlockedGlobalIndex = chosen;
        return true;
    }

    public List<EmojiProgress> GetSortedForView(int colorId, int total)
    {
        EnsureCache();

        var list = new List<EmojiProgress>(total);

        for (int i = 0; i < total; i++)
        {
            if (_cache.TryGetValue((colorId, i), out var p))
                list.Add(p);
            else
                list.Add(new EmojiProgress(colorId, i));
        }

        list.Sort((a, b) =>
        {
            if (a.IsUnlocked && b.IsUnlocked)
                return a.UnlockOrder.CompareTo(b.UnlockOrder);

            if (a.IsUnlocked) return -1;
            if (b.IsUnlocked) return 1;
            return 0;
        });

        return list;
    }

    private void EnsureCache()
    {
        if (_cache != null) return;

        _cache = new();
        foreach (var e in AllEmoji)
            _cache[(e.ColorId, e.EmojiId)] = e;
    }

    private EmojiProgress GetOrCreate(int colorId, int emojiId)
    {
        EnsureCache();

        if (_cache.TryGetValue((colorId, emojiId), out var p))
            return p;

        p = new EmojiProgress(colorId, emojiId);
        _cache[(colorId, emojiId)] = p;
        AllEmoji.Add(p);
        return p;
    }
}