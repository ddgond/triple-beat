public enum Rank
{
    F,
    D,
    C,
    B,
    A,
    S,
    SS,
    SSS
}

public static class RankMethods
{
    public static Rank ScoreToRank(int score)
    {
        if (score == 1000000)
        {
            return Rank.SSS;
        }
        if (score > 975000)
        {
            return Rank.SS;
        }
        if (score > 950000)
        {
            return Rank.S;
        }
        if (score > 900000)
        {
            return Rank.A;
        }
        if (score > 800000)
        {
            return Rank.B;
        }
        if (score > 700000)
        {
            return Rank.C;
        }
        if (score > 600000)
        {
            return Rank.D;
        }
        return Rank.F;
    }
}
