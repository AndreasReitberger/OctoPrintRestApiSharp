using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFilePrints
    {
        [JsonProperty("failure")]
        public long Failure { get; set; }

        [JsonProperty("last")]
        public OctoPrintFileLastPrint Last { get; set; }

        [JsonProperty("success")]
        public long Success { get; set; }
    }
}
