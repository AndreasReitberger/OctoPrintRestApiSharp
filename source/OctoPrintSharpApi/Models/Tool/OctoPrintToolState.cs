using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintToolState
    {
        [JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Tool0 { get; set; }

        [JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Tool1 { get; set; }

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintToolStateHistory[] History { get; set; }
    }
}
