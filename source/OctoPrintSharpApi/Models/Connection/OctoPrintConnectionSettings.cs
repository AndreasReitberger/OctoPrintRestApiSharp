using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintConnectionSettings
    {
        [JsonProperty("current")]
        public OctoPrintConnectionSettingsConnection Current { get; set; }

        [JsonProperty("options")]
        public OctoPrintConnectionSettingsOptions Options { get; set; }
    }
}
