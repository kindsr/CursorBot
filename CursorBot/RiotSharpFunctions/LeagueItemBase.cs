using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CursorBot.RiotSharpFunctions
{

    /// <summary>
    /// A League item
    /// </summary>
    public class LeagueItem
    {
        internal LeagueItem() { }

        /// <summary>
        /// The rank of the participant in a league.
        /// </summary>
        [JsonProperty("rank")]
        public string Rank { get; set; }

        /// <summary>
        /// Specifies if the participant is inactive.
        /// </summary>
        [JsonProperty("inactive")]
        public bool Inactive { get; set; }

        /// <summary>
        /// Specifies if the participant is a veteran.
        /// </summary>
        [JsonProperty("veteran")]
        public bool Veteran { get; set; }

        /// <summary>
        /// The league points of the participant.
        /// </summary>
        [JsonProperty("leaguePoints")]
        public int LeaguePoints { get; set; }

        /// <summary>
        /// The number of losses for the participant.
        /// </summary>
        [JsonProperty("losses")]
        public int Losses { get; set; }

        /// <summary>
        /// The name of the the summoner represented by this entry.
        /// </summary>
        [JsonProperty("summonerName")]
        public string SummonerName { get; set; }

        /// <summary>
        /// The encrypted id of the the summoner represented by this entry.
        /// </summary>
        [JsonProperty("summonerId")]
        public string SummonerId { get; set; }

        /// <summary>
        /// The number of wins for the participant.
        /// </summary>
        [JsonProperty("wins")]
        public int Wins { get; set; }
    }
}
