using Newtonsoft.Json;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class SettingsSlic3R
    {
        #region Properties
        [JsonProperty("debug_logging", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DebugLogging { get; set; }

        [JsonProperty("default_profile")]
        public object DefaultProfile { get; set; }

        [JsonProperty("slic3r_engine")]
        public object Slic3REngine { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
