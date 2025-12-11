using UnityEngine;

public static class StorageExtensions
{
    public static void SaveJson<T>(this IDataStorage storage, string key, T data)
    {
        storage.SaveString(key, JsonUtility.ToJson(data, true));
        storage.Save();
    }

    public static T LoadJson<T>(this IDataStorage storage, string key) where T : new()
    {
        if (!storage.HasKey(key)) return new T();
        return JsonUtility.FromJson<T>(storage.LoadString(key));
    }
}