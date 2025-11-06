using System;

public static class GameEvents
{
    public static event Action<GameMode> OnGameModeChanged;

    public static void RaiseGameModeChanged(GameMode mode)
    {
        OnGameModeChanged?.Invoke(mode);
    }
}
