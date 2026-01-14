using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class GameProgress
{
    public List<EmojiProgress> AllEmoji = new();

    [NonSerialized]
    private Dictionary<(int colorId, int emojiId), EmojiProgress> _cache;

    public int UnlockCounter;

    public int? LastUnlockedEmojiId { get; private set; }

    public bool HasAnyProgress() => AllEmoji.Count > 0;

    public bool TryGetLastUnlockedEmoji(out int emojiId)
    {
        if (!LastUnlockedEmojiId.HasValue)
        {
            emojiId = default;
            return false;
        }

        emojiId = LastUnlockedEmojiId.Value;
        return true;
    }

    public bool IsEmojiUnlocked(int colorId, int emojiId)
    {
        EnsureCache();
        return _cache.TryGetValue((colorId, emojiId), out var p) && p.IsUnlocked;
    }

    public bool IsRangeFullyUnlocked(List<EmojiData> sets, RewardRule rule)
    {
        EnsureCache();

        foreach (var set in sets)
        {
            int max = Mathf.Min(rule.To, set.EmojiSprites.Count - 1);

            for (int emojiId = rule.From; emojiId <= max; emojiId++)
            {
                if (!IsEmojiUnlocked(set.ColorId, emojiId))
                    return false;
            }
        }

        return true;
    }

    public bool UnlockNextInRange(List<EmojiData> sets, RewardRule rule)
    {
        EnsureCache();

        for (int emojiId = rule.From; emojiId <= rule.To; emojiId++)
        {
            bool unlockedAny = false;

            foreach (var set in sets)
            {
                if (emojiId >= set.EmojiSprites.Count)
                    continue;

                if (!IsEmojiUnlocked(set.ColorId, emojiId))
                {
                    UnlockInternal(set.ColorId, emojiId);
                    unlockedAny = true;
                }
            }

            if (unlockedAny)
            {
                LastUnlockedEmojiId = emojiId;
                return true;
            }
        }

        return false;
    }

    public void UnlockFirstNAllColors(Dictionary<int, int> totalPerColor, int count)
    {
        EnsureCache();

        foreach (var (colorId, total) in totalPerColor)
        {
            int limit = Mathf.Min(count, total);

            for (int emojiId = 0; emojiId < limit; emojiId++)
                UnlockInternal(colorId, emojiId);
        }

        LastUnlockedEmojiId = null;
    }

    public bool UnlockRandomLocked(List<EmojiData> sets)
    {
        EnsureCache();

        var locked = new List<int>();

        int max = 0;
        foreach (var set in sets)
            max = Mathf.Max(max, set.EmojiSprites.Count);

        for (int emojiId = 0; emojiId < max; emojiId++)
        {
            foreach (var set in sets)
            {
                if (emojiId < set.EmojiSprites.Count &&
                    !IsEmojiUnlocked(set.ColorId, emojiId))
                {
                    locked.Add(emojiId);
                    break;
                }
            }
        }

        if (locked.Count == 0)
            return false;

        int chosen = locked[UnityEngine.Random.Range(0, locked.Count)];

        foreach (var set in sets)
        {
            if (chosen < set.EmojiSprites.Count)
                UnlockInternal(set.ColorId, chosen);
        }

        LastUnlockedEmojiId = chosen;
        return true;
    }

    public List<EmojiProgress> GetSortedForView(int colorId, int total)
    {
        EnsureCache();

        var list = new List<EmojiProgress>(total);

        for (int emojiId = 0; emojiId < total; emojiId++)
        {
            if (_cache.TryGetValue((colorId, emojiId), out var p))
                list.Add(p);
            else
                list.Add(new EmojiProgress(colorId, emojiId));
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

    private void UnlockInternal(int colorId, int emojiId)
    {
        var progress = GetOrCreate(colorId, emojiId);
        if (progress.IsUnlocked)
            return;

        progress.IsUnlocked = true;
        progress.UnlockOrder = UnlockCounter++;
    }

    private void EnsureCache()
    {
        if (_cache != null)
            return;

        _cache = new();
        foreach (var p in AllEmoji)
            _cache[(p.ColorId, p.EmojiId)] = p;
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