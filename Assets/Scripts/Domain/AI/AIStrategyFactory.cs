public static class AIStrategyFactory
{
    public static IAIStrategy Create(AIStrategyType type)
    {
        return type switch
        {
            AIStrategyType.Easy => new EasyStrategy(),
            AIStrategyType.Norm => new NormalStrategy(),
            AIStrategyType.Hard => new HardStrategy(),
            _ => new EasyStrategy()
        };
    }
}