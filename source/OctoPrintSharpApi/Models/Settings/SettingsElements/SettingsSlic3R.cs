using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsSlic3R : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("debug_logging", NullValueHandling = NullValueHandling.Ignore)]
        public bool? debugLogging;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("default_profile")]
        public object? defaultProfile;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("slic3r_engine")]
        public object? slic3REngine;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
