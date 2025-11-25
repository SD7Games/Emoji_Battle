using UnityEngine;

public static class StorageExtensions
{
    public static void SaveJson(this IStorage storage, string key, object data)
    {
        string json = JsonUtility.ToJson(data);
        storage.SaveString(key, json);
    }

    public static T LoadJson<T>(this IStorage storage, string key) where T : new()
    {
        string json = storage.LoadString(key, "");
        if (string.IsNullOrEmpty(json))
            return new T();

        return JsonUtility.FromJson<T>(json);
    }
}