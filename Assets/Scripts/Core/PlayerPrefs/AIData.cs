using System;

[Serializable]
public class AIData
{
    public int Difficulty = 1;
    public int EmojiColor = 0;
    public int EmojiIndex = 0;
    public string Strategy = "Easy";

    public void Save(IStorage storage)
    {
        storage.SaveInt("AI_Difficulty", Difficulty);
        storage.SaveInt("AI_EmojiColor", EmojiColor);
        storage.SaveInt("AI_EmojiIndex", EmojiIndex);
        storage.SaveString("AI_Strategy", Strategy);
    }

    public void Load(IStorage storage)
    {
        Difficulty = storage.LoadInt("AI_Difficulty", Difficulty);
        EmojiColor = storage.LoadInt("AI_EmojiColor", EmojiColor);
        EmojiIndex = storage.LoadInt("AI_EmojiIndex", EmojiIndex);
        Strategy = storage.LoadString("AI_Strategy", "Easy");
    }
}
