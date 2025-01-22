using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsActionCommandPrompt : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Command { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("enable", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Enable { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("enable_emergency_sending", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? EnableEmergencySending { get; set; }

        [ObservableProperty]

        [JsonProperty("enable_signal_support", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? EnableSignalSupport { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
