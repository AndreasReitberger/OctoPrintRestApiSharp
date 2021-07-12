using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintConnectionSettingsPrinterProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
