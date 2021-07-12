using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterStateHistory
    {
        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Bed { get; set; }

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public long? Time { get; set; }

        [JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Tool0 { get; set; }

        [JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Tool1 { get; set; }
    }
}
