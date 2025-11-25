using UnityEngine;

public class PlayerPrefsStorage : IStorage
{
    public void SaveString(string key, string value) => PlayerPrefs.SetString(key, value);
    public string LoadString(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);

    public void SaveInt(string key, int value) => PlayerPrefs.SetInt(key, value);
    public int LoadInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);

    public bool HasKey(string key) => PlayerPrefs.HasKey(key);
    public void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);

    public void Save() => PlayerPrefs.Save();

    public void Clear()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
