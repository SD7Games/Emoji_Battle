using UnityEngine;

public class PlayerPrefsStorage : IDataStorage
{
    public void SaveString(string key, string value) => PlayerPrefs.SetString(key, value);

    public string LoadString(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);

    public bool HasKey(string key) => PlayerPrefs.HasKey(key);

    public void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);

    public void Save() => PlayerPrefs.Save();
}