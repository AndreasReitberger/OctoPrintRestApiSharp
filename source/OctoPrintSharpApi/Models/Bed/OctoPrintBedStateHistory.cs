using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintBedStateHistory
    {
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public long? Time { get; set; }

        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Bed { get; set; }
    }
}
