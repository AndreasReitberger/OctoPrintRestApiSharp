using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class Errortracking
    {
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("enabled_unreleased", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnabledUnreleased { get; set; }

        [JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? UniqueId { get; set; }

        [JsonProperty("url_coreui", NullValueHandling = NullValueHandling.Ignore)]
        public Uri UrlCoreui { get; set; }

        [JsonProperty("url_server", NullValueHandling = NullValueHandling.Ignore)]
        public Uri UrlServer { get; set; }
    }
}
