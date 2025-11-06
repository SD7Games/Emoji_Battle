using System;
using UnityEngine;

public static class AISettingManager
{
    public static void SetGameMode(GameMode mode)
    {
        PlayerPrefs.SetString("GameMode", mode.ToString());
    }

    public static GameMode GetGameMode()
    {
        string modeString = PlayerPrefs.GetString("GameMode", GameMode.PVE.ToString());
        return (GameMode) Enum.Parse(typeof(GameMode), modeString);
    }

    public static class Player
    {
        public static void SetName(string name) => PlayerPrefs.SetString("PlayerName", name);
        public static string GetName() => PlayerPrefs.GetString("PlayerName", "Player");

        public static void SetEmojiIndex(int index) => PlayerPrefs.SetInt("PlayerEmojiIndex", index);
        public static int GetEmojiIndex() => PlayerPrefs.GetInt("PlayerEmojiIndex", 0);

        public static void SetEmojiColor(string colorName) => PlayerPrefs.SetString("PlayerEmojiColor", colorName);
        public static string GetEmojiColor() => PlayerPrefs.GetString("PlayerEmojiColor", "Default");
    }

    public static class AI
    {
        public static void SetDifficulty(int diff) => PlayerPrefs.SetInt("AIDifficulty", diff);
        public static int GetDifficulty() => PlayerPrefs.GetInt("AIDifficulty", 1);

        public static void SetEmojiAIIndex(int index) => PlayerPrefs.SetInt("AIEmojiIndex", index);
        public static int GetEmojiAIIndex() => PlayerPrefs.GetInt("AIEmojiIndex", 0);

        public static void SetEmojiAIColor(string colorName) => PlayerPrefs.SetString("AIEmojiColor", colorName);
        public static string GetEmojiAIColor() => PlayerPrefs.GetString("AIEmojiColor", "Default");

        public static void SetStrategy(string strategy) => PlayerPrefs.SetString("AIStrategy", strategy);
        public static string GetStrategy() => PlayerPrefs.GetString("AIStrategy", "Easy");
    }

    public static void Save() => PlayerPrefs.Save();

    public static void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
