using System;

[Serializable]
public class AIData
{
    public int Difficulty = 1;
    public string EmojiColor = "Default";
    public int EmojiIndex = 0;
    public string Strategy = "Easy";

    public void Save(IStorage storage)
    {
        storage.SaveInt("AI_Difficulty", Difficulty);
        storage.SaveString("AI_EmojiColor", EmojiColor);
        storage.SaveInt("AI_EmojiIndex", EmojiIndex);
        storage.SaveString("AI_Strategy", Strategy);
    }

    public void Load(IStorage storage)
    {
        Difficulty = storage.LoadInt("AI_Difficulty", 1);
        EmojiColor = storage.LoadString("AI_EmojiColor", "Default");
        EmojiIndex = storage.LoadInt("AI_EmojiIndex", 0);
        Strategy = storage.LoadString("AI_Strategy", "Easy");
    }
}
