using System;

public sealed class SettingsService
{
    private static SettingsService _instance;
    public static SettingsService I => _instance ??= new SettingsService();

    public static event Action<string> PlayerNameChanged;

    public static event Action MusicChanged;

    public static event Action SfxChanged;

    public static event Action VibrationChanged;

    private SettingsService()
    { }

    public SettingsData Data => GameDataService.I.Data.Settings;

    public void SetMusicEnabled(bool enabled)
    {
        Data.MusicEnabled = enabled;
        GameDataService.I.Save();
        MusicChanged?.Invoke();
    }

    public void SetMusicVolume(float value)
    {
        Data.MusicVolume = value;
        GameDataService.I.Save();
        MusicChanged?.Invoke();
    }

    public void SetSfxEnabled(bool enabled)
    {
        Data.SfxEnabled = enabled;
        GameDataService.I.Save();
        SfxChanged?.Invoke();
    }

    public void SetSfxVolume(float value)
    {
        Data.SfxVolume = value;
        GameDataService.I.Save();
        SfxChanged?.Invoke();
    }

    public void SetVibration(bool enabled)
    {
        Data.VibrationEnabled = enabled;
        GameDataService.I.Save();
        VibrationChanged?.Invoke();
    }

    public void SetPlayerName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            name = "Player";

        name = name.Trim();

        if (name.Length > 8)
            name = name.Substring(0, 8);

        GameDataService.I.Data.Player.Name = name;
        GameDataService.I.Save();

        PlayerNameChanged?.Invoke(name);
    }
}