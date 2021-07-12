using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterAxesAttribute
    {
        [JsonProperty("inverted")]
        public bool Inverted { get; set; }

        [JsonProperty("speed")]
        public long Speed { get; set; }
    }
}
