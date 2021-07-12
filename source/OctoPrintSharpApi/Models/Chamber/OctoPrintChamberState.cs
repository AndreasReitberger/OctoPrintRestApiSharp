using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintChamberState
    {
        [JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Chamber { get; set; }

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintChamberStateHistory[] History { get; set; }
    }
}
