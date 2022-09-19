using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintConnectionSettingsOptions
    {
        #region Properties
        [JsonProperty("baudratePreference")]
        public object BaudratePreference { get; set; }

        [JsonProperty("baudrates")]
        public List<long> Baudrates { get; set; } = new();

        [JsonProperty("portPreference")]
        public object PortPreference { get; set; }

        [JsonProperty("ports")]
        public List<string> Ports { get; set; } = new();

        [JsonProperty("printerProfilePreference")]
        public string PrinterProfilePreference { get; set; }

        [JsonProperty("autoconnect", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Autoconnect { get; set; }

        [JsonProperty("printerProfiles")]
        public List<OctoPrintConnectionSettingsPrinterProfile> PrinterProfiles { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
