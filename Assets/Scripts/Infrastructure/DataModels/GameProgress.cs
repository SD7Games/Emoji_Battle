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

    public int NextWinEasy;
    public int NextWinNorm;
    public int NextWinHard;

    public int? LastUnlockedEmojiId { get; private set; }

    private const int START_OPEN_COUNT = 4;

    public bool IsEmojiUnlocked(int colorId, int emojiId)
    {
        EnsureCache();
        return _cache.TryGetValue((colorId, emojiId), out var p) && p.IsUnlocked;
    }

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

    public void UnlockFirstNAllColors(
        Dictionary<int, int> totalPerColor,
        int count)
    {
        EnsureCache();

        foreach (var pair in totalPerColor)
        {
            int colorId = pair.Key;
            int total = pair.Value;

            int limit = Mathf.Min(count, total);

            for (int emojiId = 0; emojiId < limit; emojiId++)
                UnlockInternal(colorId, emojiId);
        }

        LastUnlockedEmojiId = null;
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

        if (UnlockCounter == 0)
        {
            list.Sort((a, b) => a.EmojiId.CompareTo(b.EmojiId));
            return list;
        }

        list.Sort((a, b) =>
        {
            bool aStart = a.EmojiId < START_OPEN_COUNT;
            bool bStart = b.EmojiId < START_OPEN_COUNT;

            if (aStart != bStart)
                return aStart ? -1 : 1;

            if (aStart && bStart)
                return a.EmojiId.CompareTo(b.EmojiId);

            if (a.IsUnlocked != b.IsUnlocked)
                return a.IsUnlocked ? -1 : 1;

            if (a.IsUnlocked && b.IsUnlocked)
                return a.UnlockOrder.CompareTo(b.UnlockOrder);

            return a.EmojiId.CompareTo(b.EmojiId);
        });

        return list;
    }

    public bool UnlockNextWinByDifficulty(
        AIStrategyType difficulty,
        List<EmojiData> sets,
        RewardRule rule)
    {
        EnsureCache();

        if (rule.To < rule.From)
            return false;

        int pointer = GetPointer(difficulty);

        if (pointer < rule.From)
            pointer = rule.From;

        while (pointer <= rule.To)
        {
            int emojiId = pointer;

            bool alreadyUnlockedEverywhere = true;

            foreach (var set in sets)
            {
                if (set == null || set.EmojiSprites == null)
                    continue;

                if (emojiId >= set.EmojiSprites.Count)
                    continue;

                if (!IsEmojiUnlocked(set.ColorId, emojiId))
                {
                    alreadyUnlockedEverywhere = false;
                    break;
                }
            }

            if (alreadyUnlockedEverywhere)
            {
                pointer++;
                continue;
            }

            foreach (var set in sets)
            {
                if (set == null || set.EmojiSprites == null)
                    continue;

                if (emojiId < set.EmojiSprites.Count)
                    UnlockInternal(set.ColorId, emojiId);
            }

            LastUnlockedEmojiId = emojiId;

            pointer++;
            SetPointer(difficulty, pointer);
            return true;
        }

        SetPointer(difficulty, rule.To + 1);
        return false;
    }

    public bool UnlockRandomLocked(List<EmojiData> sets)
    {
        EnsureCache();

        var lockedIds = new List<int>();
        int max = 0;

        foreach (var set in sets)
        {
            if (set == null || set.EmojiSprites == null) continue;
            max = Mathf.Max(max, set.EmojiSprites.Count);
        }

        for (int emojiId = 0; emojiId < max; emojiId++)
        {
            foreach (var set in sets)
            {
                if (set == null || set.EmojiSprites == null) continue;

                if (emojiId < set.EmojiSprites.Count &&
                    !IsEmojiUnlocked(set.ColorId, emojiId))
                {
                    lockedIds.Add(emojiId);
                    break;
                }
            }
        }

        if (lockedIds.Count == 0)
            return false;

        int chosen = lockedIds[UnityEngine.Random.Range(0, lockedIds.Count)];

        foreach (var set in sets)
        {
            if (set == null || set.EmojiSprites == null) continue;

            if (chosen < set.EmojiSprites.Count)
                UnlockInternal(set.ColorId, chosen);
        }

        LastUnlockedEmojiId = chosen;
        return true;
    }

    public bool IsAllUnlocked(List<EmojiData> sets)
    {
        EnsureCache();

        foreach (var set in sets)
        {
            if (set == null || set.EmojiSprites == null) continue;

            for (int i = 0; i < set.EmojiSprites.Count; i++)
            {
                if (!IsEmojiUnlocked(set.ColorId, i))
                    return false;
            }
        }

        return true;
    }

    private void UnlockInternal(int colorId, int emojiId)
    {
        var p = GetOrCreate(colorId, emojiId);
        if (p.IsUnlocked)
            return;

        p.IsUnlocked = true;
        p.UnlockOrder = UnlockCounter++;
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
        _cache[(p.ColorId, emojiId)] = p;
        AllEmoji.Add(p);
        return p;
    }

    private int GetPointer(AIStrategyType difficulty) =>
        difficulty switch
        {
            AIStrategyType.Easy => NextWinEasy,
            AIStrategyType.Norm => NextWinNorm,
            AIStrategyType.Hard => NextWinHard,
            _ => NextWinEasy
        };

    private void SetPointer(AIStrategyType difficulty, int value)
    {
        switch (difficulty)
        {
            case AIStrategyType.Easy: NextWinEasy = value; break;
            case AIStrategyType.Norm: NextWinNorm = value; break;
            case AIStrategyType.Hard: NextWinHard = value; break;
            default: NextWinEasy = value; break;
        }
    }
}