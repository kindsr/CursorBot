using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CursorBot.RiotSharpFunctions
{
    /// <summary>
    /// Class representing a champion (Static API).
    /// </summary>
    public class ChampionStatic
    {
        internal ChampionStatic() { }

        /// <summary>
        /// List of tips to use while playing this champion.
        /// </summary>
        [JsonProperty("allytips")]
        public List<string> AllyTips { get; set; }

        /// <summary>
        /// Beginning of the lore.
        /// </summary>
        [JsonProperty("blurb")]
        public string Blurb { get; set; }

        /// <summary>
        /// List of tips to use while playing against this champion.
        /// </summary>
        [JsonProperty("enemytips")]
        public List<string> EnemyTips { get; set; }

        /// <summary>
        /// Id of this champion.
        /// Taken from key field to remain consistent with the old static data api.
        /// </summary>
        [JsonProperty("key")]
        public int Id { get; set; }

        /// <summary>
        /// Key of this champion.
        /// Taken from key field to remain consistent with the old static data api.
        /// <para>This is diffrent from the Name attribute!
        /// (Name = ingame display name, Key = codebase name
        /// [Fiddlesticks key = FiddleSticks, Wukong key = MonkeyKing, ... ]</para>
        /// </summary>
        [JsonProperty("id")]
        public string Key { get; set; }

        /// <summary>
        /// Lore of this champion.
        /// </summary>
        [JsonProperty("lore")]
        public string Lore { get; set; }

        /// <summary>
        /// Name of this champion.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Resource type of the champion (Mana, None, Energy, Shield, Rage, Ferocity, Heat, Dragonfury, Battlefury,
        /// Wind).
        /// </summary>
        [JsonProperty("partype")]
        public string Partype { get; set; }

    }
}
