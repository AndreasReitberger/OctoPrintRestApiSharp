using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterState
    {
        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperature Temperature { get; set; }

        [JsonProperty("sd", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateSd Sd { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateState State { get; set; }
    }
}
