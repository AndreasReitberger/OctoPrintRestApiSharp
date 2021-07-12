using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintVersionInfo
    {
        [JsonProperty("api")]
        public string Api { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
