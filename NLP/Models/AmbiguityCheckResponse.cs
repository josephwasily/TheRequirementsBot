using Newtonsoft.Json;

namespace MultiTurnPromptBot.NLP.Models
{
    public class AmbiguityCheckResponse
    {
        [JsonProperty("ambigious")]
        public bool Ambiguous { get; set; }

        [JsonProperty("ambiguousWords")]
        public string[] AmbiguousWords { get; set; }

        [JsonProperty("rewrite")]
        public string Rewrite { get; set; }

    }
}
