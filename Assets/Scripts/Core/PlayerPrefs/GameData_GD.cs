using UnityEngine;

public static class GD
{
    private static IStorage _storage;
    private static PlayerData _player;
    private static AIData _ai;

    private static PlayerProgress _playerProgress = new PlayerProgress();
    public static PlayerProgress PlayerProgress => _playerProgress;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        _storage = new PlayerPrefsStorage();
        _player = new PlayerData();
        _ai = new AIData();

        Load();
    }

    public static PlayerData Player => _player;
    public static AIData AI => _ai;

    public static void Save()
    {
        _player.Save(_storage);
        _ai.Save(_storage);
        SaveProgress();
        _storage.Save();
    }

    public static void SaveProgress()
    {
        _storage.SaveJson("player_progress", _playerProgress);
    }

    public static void Load()
    {
        _player.Load(_storage);
        _ai.Load(_storage);

        if (_storage.HasKey("player_progress"))
            _playerProgress = _storage.LoadJson<PlayerProgress>("player_progress");
    }
}
