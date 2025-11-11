using System;
using UnityEngine;

public enum GameMode
{
    PvE,
    PvP
}

public static class GD
{
    private static IStorage _storage;
    private static PlayerData _player;
    private static PlayerData _player2;
    private static AIData _ai;
    private static GameMode _mode;

    // 🟡 Событие при смене режима игры
    public static event Action<GameMode> OnGameModeChanged;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        _storage = new PlayerPrefsStorage();

        _player = new PlayerData("Player");
        _player2 = new PlayerData("Player2");
        _ai = new AIData();

        Load();

        Debug.Log($"[GD] Initialized with mode {_mode}");
    }

    public static PlayerData Player => _player;
    public static PlayerData Player2 => _player2;
    public static AIData AI => _ai;

    public static GameMode Mode
    {
        get => _mode;
        set
        {
            if (_mode == value) return; // ничего не делаем, если режим не поменялся

            _mode = value;
            _storage.SaveString("GameMode", _mode.ToString());
            _storage.Save();

            Debug.Log($"[GD] Mode switched to {_mode}");

            // 🔥 Вызов события (оповещаем всех подписчиков)
            OnGameModeChanged?.Invoke(_mode);
        }
    }

    public static void Save()
    {
        _player.Save(_storage);
        _player2.Save(_storage);
        _ai.Save(_storage);
        _storage.SaveString("GameMode", _mode.ToString());
        _storage.Save();
        Debug.Log("[GD] Saved all data");
    }

    public static void Load()
    {
        _player.Load(_storage);
        _player2.Load(_storage);
        _ai.Load(_storage);

        string modeString = _storage.LoadString("GameMode", GameMode.PvE.ToString());

        if (!Enum.TryParse(modeString, true, out _mode))
        {
            _mode = GameMode.PvE;
            Debug.LogWarning($"[GD] Invalid stored GameMode '{modeString}', fallback to PvE");
        }

        Debug.Log($"[GD] Loaded mode {_mode}");
    }


    public static void ResetAll()
    {
        _storage.Clear();
        _player = new PlayerData("Player");
        _player2 = new PlayerData("Player2");
        _ai = new AIData();
        _mode = GameMode.PvE;
        Debug.Log("[GD] Reset all data");

        // уведомим слушателей, что режим сброшен
        OnGameModeChanged?.Invoke(_mode);
    }
}
