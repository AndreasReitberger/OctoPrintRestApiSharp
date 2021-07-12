using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class GcodeAnalysis
    {
        [JsonProperty("runAt", NullValueHandling = NullValueHandling.Ignore)]
        public string RunAt { get; set; }
    }
}
