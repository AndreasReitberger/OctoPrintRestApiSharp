using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPiSupport : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("vcgencmd_throttle_check_command", NullValueHandling = NullValueHandling.Ignore)]
        public partial string VcgencmdThrottleCheckCommand { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("vcgencmd_throttle_check_enabled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? VcgencmdThrottleCheckEnabled { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
