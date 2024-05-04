public class GoldCoinCalculator
{
    public enum GoldEarners
    {
        teamWon
    }

    public static int CalculateGoldCoinReturn(GoldEarners earnMethod)
    {
        switch (earnMethod)
        {
            case GoldEarners.teamWon:
                return 100;
            default:
                return 0;
        }
    }
}
