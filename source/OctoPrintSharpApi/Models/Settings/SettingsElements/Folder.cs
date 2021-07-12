using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class Folder
    {
        [JsonProperty("logs", NullValueHandling = NullValueHandling.Ignore)]
        public string Logs { get; set; }

        [JsonProperty("timelapse", NullValueHandling = NullValueHandling.Ignore)]
        public string Timelapse { get; set; }

        [JsonProperty("timelapseTmp", NullValueHandling = NullValueHandling.Ignore)]
        public string TimelapseTmp { get; set; }

        [JsonProperty("uploads", NullValueHandling = NullValueHandling.Ignore)]
        public string Uploads { get; set; }

        [JsonProperty("watched", NullValueHandling = NullValueHandling.Ignore)]
        public string Watched { get; set; }
    }
}
