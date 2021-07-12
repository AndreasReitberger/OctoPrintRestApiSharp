using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterExtruder
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("nozzleDiameter")]
        public double NozzleDiameter { get; set; }

        [JsonProperty("offsets")]
        public long[][] Offsets { get; set; }

        [JsonProperty("sharedNozzle")]
        public bool SharedNozzle { get; set; }
    }
}
