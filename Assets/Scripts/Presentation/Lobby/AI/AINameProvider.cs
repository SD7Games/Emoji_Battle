using UnityEngine;

public static class AINameProvider
{
    private static readonly string[] _easyNames =
    {
        "Bobby", "Sunny", "Chippy", "Goober", "Smiley",
        "Cookie", "Dolly", "Peanut", "Bubbles", "Buddy"
    };

    private static readonly string[] _normNames =
    {
        "Vector", "Omega", "Tracer", "Razor", "Nova",
        "Pilot", "Orbit", "Cipher", "Dash", "Pulse"
    };

    private static readonly string[] _hardNames =
    {
        "Nemesis", "Overlord", "Vortex", "Harbinger", "Phantom",
        "Titan", "Oblivion", "Reaper", "Nightfall", "Dominator"
    };

    public static string GetRandomName(AIStrategyType type)
    {
        switch (type)
        {
            case AIStrategyType.Easy:
                return _easyNames[Random.Range(0, _easyNames.Length)];

            case AIStrategyType.Norm:
                return _normNames[Random.Range(0, _normNames.Length)];

            case AIStrategyType.Hard:
                return _hardNames[Random.Range(0, _hardNames.Length)];

            default:
                return "AI";
        }
    }
}