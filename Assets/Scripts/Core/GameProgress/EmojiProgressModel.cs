using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class EmojiProgressModel
{
    public List<EmojiProgress> All = new List<EmojiProgress>();
   
    public EmojiProgress GetProgress(int colorId, int emojiId)
    {
        return All.Find(e => e.ColorId == colorId && e.EmojiId == emojiId);
    }
    
    public void AddProgress(int colorId, int emojiId, bool unlocked = false)
    {
        if (GetProgress(colorId, emojiId) == null)
            All.Add(new EmojiProgress(colorId, emojiId, unlocked));
    }
   
    public void Unlock(int colorId, int emojiId)
    {
        var progress = GetProgress(colorId, emojiId);
        if (progress != null)
        {
            progress.IsUnlocked = true;
            return;
        }

        All.Add(new EmojiProgress(colorId, emojiId, true));
    }
   
    public bool IsUnlocked(int colorId, int emojiId)
    {
        var progress = GetProgress(colorId, emojiId);
        return progress != null && progress.IsUnlocked;
    }
    
    public void UnlockFirstN(int colorId, int count)
    {
        int unlocked = 0;

        var ordered = All
            .Where(e => e.ColorId == colorId)
            .OrderBy(e => e.EmojiId);

        foreach (var emoji in ordered)
        {
            if (!emoji.IsUnlocked)
            {
                emoji.IsUnlocked = true;
                unlocked++;

                if (unlocked >= count)
                    break;
            }
        }
    }
   
    public EmojiProgress UnlockRandomLocked(int colorId)
    {
        var locked = All.Where(e => e.ColorId == colorId && !e.IsUnlocked).ToList();

        if (locked.Count == 0)
            return null;

        var rnd = locked[UnityEngine.Random.Range(0, locked.Count)];
        rnd.IsUnlocked = true;
        return rnd;
    }
}
