using Newtonsoft.Json;

namespace CursorBot
{
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
        [JsonProperty("riotapikey")]
        public string Riotapikey { get; private set; }
        [JsonProperty("version")]
        public string Version { get; private set; }
    }
}
