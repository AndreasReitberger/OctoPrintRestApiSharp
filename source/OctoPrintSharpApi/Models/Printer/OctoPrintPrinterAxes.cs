using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterAxes
    {
        [JsonProperty("e")]
        public OctoPrintPrinterAxesAttribute E { get; set; }

        [JsonProperty("x")]
        public OctoPrintPrinterAxesAttribute X { get; set; }

        [JsonProperty("y")]
        public OctoPrintPrinterAxesAttribute Y { get; set; }

        [JsonProperty("z")]
        public OctoPrintPrinterAxesAttribute Z { get; set; }
    }
}
