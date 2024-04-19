using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsActionCommandPrompt : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        string command = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable", NullValueHandling = NullValueHandling.Ignore)]
        string enable = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_emergency_sending", NullValueHandling = NullValueHandling.Ignore)]
        bool? enableEmergencySending;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_signal_support", NullValueHandling = NullValueHandling.Ignore)]
        bool? enableSignalSupport;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
