using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFilePrintTime
    {
        [JsonProperty("_default")]
        public double Default { get; set; }
    }
}
