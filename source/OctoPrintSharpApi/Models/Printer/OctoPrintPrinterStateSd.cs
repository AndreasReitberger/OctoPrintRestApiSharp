using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterStateSd
    {
        [JsonProperty("ready", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Ready { get; set; }
    }
}
