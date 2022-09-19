using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPiSupport
    {
        #region Properties
        [JsonProperty("vcgencmd_throttle_check_command", NullValueHandling = NullValueHandling.Ignore)]
        public string VcgencmdThrottleCheckCommand { get; set; }

        [JsonProperty("vcgencmd_throttle_check_enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? VcgencmdThrottleCheckEnabled { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
