using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileLastPrint
    {
        [JsonProperty("date")]
        public double Date { get; set; }

        [JsonProperty("printTime")]
        public double PrintTime { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
