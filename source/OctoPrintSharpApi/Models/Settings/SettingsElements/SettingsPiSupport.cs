using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPiSupport : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("vcgencmd_throttle_check_command", NullValueHandling = NullValueHandling.Ignore)]
        string vcgencmdThrottleCheckCommand = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("vcgencmd_throttle_check_enabled", NullValueHandling = NullValueHandling.Ignore)]
        bool? vcgencmdThrottleCheckEnabled;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
