using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CursorBot.RiotSharpFunctions
{
    public class JoinedMember
    {
        public JoinedMember()
        {
        }

        public string SummonerName { get; set; }
        public string SummonerID { get; set; }
        public string Tier { get; set; }
        public string Rank { get; set; }
        public int LeaguePoints { get; set; }
        public int TierScore { get; set; }
        public string MostChampions { get; set; }
        public int Team { get; set; }
        public int TeamOrder { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({1}-{2}) ", SummonerName, Tier, Rank);
            //return string.Format("{0} ({1})", SummonerName, MostChampions.Aggregate((a, b) => a + "," + b));
        }
    }
}
