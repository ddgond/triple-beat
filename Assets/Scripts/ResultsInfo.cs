using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResultsInfo
{
    public static int misses;
    public static int goodHits;
    public static int perfectHits;
    public static int score;
    public static int combo;

    public static void Clear()
    {
        misses = 0;
        goodHits = 0;
        perfectHits = 0;
        score = 0;
        combo = 0;
    }
}
