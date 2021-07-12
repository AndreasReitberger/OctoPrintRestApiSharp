using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterFilament
    {
        [JsonProperty("length")]
        public double Length { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }
    }
}
