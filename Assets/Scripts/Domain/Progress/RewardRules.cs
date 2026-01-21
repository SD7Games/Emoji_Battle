public readonly struct RewardRule
{
    public readonly int From;
    public readonly int To;

    public RewardRule(int from, int to)
    {
        From = from;
        To = to;
    }
}

public static class RewardRules
{
    public static RewardRule ByDifficulty(AIStrategyType diff)
    {
        return diff switch
        {
            AIStrategyType.Easy => new RewardRule(3, 19),
            AIStrategyType.Norm => new RewardRule(20, 39),
            AIStrategyType.Hard => new RewardRule(40, 86),
            _ => new RewardRule(0, -1)
        };
    }
}