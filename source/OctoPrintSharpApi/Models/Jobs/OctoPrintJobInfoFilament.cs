using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintJobInfoFilament
    {
        [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
        public long Length { get; set; }

        [JsonProperty("volume", NullValueHandling = NullValueHandling.Ignore)]
        public double Volume { get; set; }
    }
}
