public interface IStorage
{
    void SaveString(string key, string value);
    string LoadString(string key, string defaultValue = "");

    void SaveInt(string key, int value);
    int LoadInt(string key, int defaultValue = 0);

    void Save();
    void Clear();
}
