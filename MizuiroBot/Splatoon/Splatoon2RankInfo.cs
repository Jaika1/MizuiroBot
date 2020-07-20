using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MizuiroBot.Splatoon
{
    public class Splatoon2RankInfo
    {
        public string RainmakerRank;
        public float BestRainmakerPower = 0.0f;
        
        public string SplatZonesRank;
        public float BestSplatZonesPower = 0.0f;
        
        public string TowerControlRank;
        public float BestTowerControlPower = 0.0f;
        
        public string ClamBlitzRank;
        public float BestClamBlitzPower = 0.0f;

        public float BestLeaguePower = 0.0f;

        private static string[] validRanks = new string[]
        {
            "C-",
            "C",
            "C+",
            "B-",
            "B",
            "B+",
            "A-",
            "A",
            "A+",
            "S",
            "S+",
            "S+1",
            "S+2",
            "S+3",
            "S+4",
            "S+5",
            "S+6",
            "S+7",
            "S+8",
            "S+9",
            "X"
        };
        public static bool IsValidRank(string rank)
        {
            return validRanks.Contains(rank);
        }
    }
}
