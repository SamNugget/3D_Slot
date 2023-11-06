using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class Result
    {
        public float winnings;
        public int[] reelPositions;
        public string[][] symbols;
        public LineWin[] lineWins;
    }

    public class LineWin
    {
        public float winAmount;
        public int[][] symbolPositions;
    }
}

