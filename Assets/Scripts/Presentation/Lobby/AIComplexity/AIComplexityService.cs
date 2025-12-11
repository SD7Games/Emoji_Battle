public static class AIComplexityService
{
    public static AIStrategyType Load()
    {
        return GameDataService.I.Data.AI.Strategy;
    }

    public static void Save(AIStrategyType diff)
    {
        GameDataService.I.Data.AI.Strategy = diff;
        GameDataService.I.Save();
    }
}