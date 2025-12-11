using UnityEngine;

public static class AINameProvider
{
    private static readonly string[] EasyNames =
    {
        "Bobby", "Sunny", "Chippy", "Goober", "Smiley",
        "Cookie", "Dolly", "Peanut", "Bubbles", "Buddy"
    };

    private static readonly string[] NormNames =
    {
        "Vector", "Omega", "Tracer", "Razor", "Nova",
        "Pilot", "Orbit", "Cipher", "Dash", "Pulse"
    };

    private static readonly string[] HardNames =
    {
        "Nemesis", "Overlord", "Vortex", "Harbinger", "Phantom",
        "Titan", "Oblivion", "Reaper", "Nightfall", "Dominator"
    };

    public static string GetRandomName(AIStrategyType type)
    {
        switch (type)
        {
            case AIStrategyType.Easy:
                return EasyNames[Random.Range(0, EasyNames.Length)];

            case AIStrategyType.Norm:
                return NormNames[Random.Range(0, NormNames.Length)];

            case AIStrategyType.Hard:
                return HardNames[Random.Range(0, HardNames.Length)];

            default:
                return "AI";
        }
    }
}