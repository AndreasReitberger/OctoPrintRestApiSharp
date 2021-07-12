using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintBedState
    {
        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Bed { get; set; }

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintBedStateHistory[] History { get; set; }
    }
}
