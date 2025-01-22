using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintConnectionSettingsOptions : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("baudratePreference")]
        public partial object? BaudratePreference { get; set; }

        [ObservableProperty]

        [JsonProperty("baudrates")]
        public partial List<long> Baudrates { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("portPreference")]
        public partial object? PortPreference { get; set; }

        [ObservableProperty]

        [JsonProperty("ports")]
        public partial List<string> Ports { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("printerProfilePreference")]
        public partial string PrinterProfilePreference { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("autoconnect", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Autoconnect { get; set; }

        [ObservableProperty]

        [JsonProperty("printerProfiles")]
        public partial List<OctoPrintConnectionSettingsPrinterProfile> PrinterProfiles { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
