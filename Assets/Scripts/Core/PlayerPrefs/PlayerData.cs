using System;

[Serializable]
public class PlayerData
{
    public string Name = "Player";
    public string EmojiColor = "Default";
    public int EmojiIndex = 0;

    private readonly string _prefix;

    public PlayerData(string prefix)
    {
        _prefix = prefix;
    }

    public void Save(IStorage storage)
    {
        storage.SaveString($"{_prefix}_Name", Name);
        storage.SaveString($"{_prefix}_EmojiColor", EmojiColor);
        storage.SaveInt($"{_prefix}_EmojiIndex", EmojiIndex);
    }

    public void Load(IStorage storage)
    {
        Name = storage.LoadString($"{_prefix}_Name", Name);
        EmojiColor = storage.LoadString($"{_prefix}_EmojiColor", EmojiColor);
        EmojiIndex = storage.LoadInt($"{_prefix}_EmojiIndex", EmojiIndex);
    }
}
