using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class Api
    {
        [JsonProperty("allowCrossOrigin", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowCrossOrigin { get; set; }

        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }
    }
}
