using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintActionResult
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }
    }
}
