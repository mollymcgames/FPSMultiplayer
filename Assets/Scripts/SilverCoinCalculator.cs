public class SilverCoinCalculator
{
    public enum SilverEarners
    {
        playerHit,
        playerKilled,
        playerKillAssist
    }

    public static int CalculateSilverCoinReturn(SilverEarners earnMethod)
    {
        switch (earnMethod)
        {
            case SilverEarners.playerHit:
                return 1;
            case SilverEarners.playerKilled:
                return 10;
            case SilverEarners.playerKillAssist:
                return 5;
            default:
                return 0;
        }
    }
}
