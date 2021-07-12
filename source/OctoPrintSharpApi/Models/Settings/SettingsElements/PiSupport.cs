using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class PiSupport
    {
        [JsonProperty("vcgencmd_throttle_check_command", NullValueHandling = NullValueHandling.Ignore)]
        public string VcgencmdThrottleCheckCommand { get; set; }

        [JsonProperty("vcgencmd_throttle_check_enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? VcgencmdThrottleCheckEnabled { get; set; }
    }
}
