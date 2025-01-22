using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsSlic3R : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("debug_logging", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? DebugLogging { get; set; }

        [ObservableProperty]

        [JsonProperty("default_profile")]
        public partial object? DefaultProfile { get; set; }

        [ObservableProperty]

        [JsonProperty("slic3r_engine")]
        public partial object? Slic3REngine { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
