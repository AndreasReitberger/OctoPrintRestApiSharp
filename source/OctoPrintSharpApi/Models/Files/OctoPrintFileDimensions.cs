using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileDimensions
    {
        [JsonProperty("depth")]
        public double Depth { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("width")]
        public double Width { get; set; }
    }
}
