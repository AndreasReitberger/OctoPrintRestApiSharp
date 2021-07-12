using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class ActionCommandPrompt
    {
        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        public string Command { get; set; }

        [JsonProperty("enable", NullValueHandling = NullValueHandling.Ignore)]
        public string Enable { get; set; }

        [JsonProperty("enable_emergency_sending", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableEmergencySending { get; set; }

        [JsonProperty("enable_signal_support", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableSignalSupport { get; set; }
    }
}
