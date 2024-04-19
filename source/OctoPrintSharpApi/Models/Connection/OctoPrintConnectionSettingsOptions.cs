using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintConnectionSettingsOptions : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("baudratePreference")]
        object? baudratePreference;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("baudrates")]
        List<long> baudrates = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("portPreference")]
        object? portPreference;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ports")]
        List<string> ports = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printerProfilePreference")]
        string printerProfilePreference = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("autoconnect", NullValueHandling = NullValueHandling.Ignore)]
        bool? autoconnect;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printerProfiles")]
        List<OctoPrintConnectionSettingsPrinterProfile> printerProfiles = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
