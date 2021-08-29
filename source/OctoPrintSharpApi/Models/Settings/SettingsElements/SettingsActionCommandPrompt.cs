using Newtonsoft.Json;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class SettingsActionCommandPrompt
    {
        #region Properties
        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        public string Command { get; set; }

        [JsonProperty("enable", NullValueHandling = NullValueHandling.Ignore)]
        public string Enable { get; set; }

        [JsonProperty("enable_emergency_sending", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableEmergencySending { get; set; }

        [JsonProperty("enable_signal_support", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableSignalSupport { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
