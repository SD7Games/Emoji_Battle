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
            AIStrategyType.Easy => new RewardRule(5, 20),
            AIStrategyType.Norm => new RewardRule(21, 40),
            AIStrategyType.Hard => new RewardRule(41, 87),
            _ => new RewardRule(0, 0)
        };
    }
}