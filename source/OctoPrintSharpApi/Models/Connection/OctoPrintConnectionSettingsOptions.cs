using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintConnectionSettingsOptions
    {
        [JsonProperty("baudratePreference")]
        public object BaudratePreference { get; set; }

        [JsonProperty("baudrates")]
        public long[] Baudrates { get; set; }

        [JsonProperty("portPreference")]
        public object PortPreference { get; set; }

        [JsonProperty("ports")]
        public string[] Ports { get; set; }

        [JsonProperty("printerProfilePreference")]
        public string PrinterProfilePreference { get; set; }

        [JsonProperty("autoconnect", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Autoconnect { get; set; }

        [JsonProperty("printerProfiles")]
        public List<OctoPrintConnectionSettingsPrinterProfile> PrinterProfiles { get; set; }
    }
}
