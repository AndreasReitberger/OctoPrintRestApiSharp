using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class Channels
    {
        [JsonProperty("_blog", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Blog { get; set; }

        [JsonProperty("_important", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Important { get; set; }

        [JsonProperty("_octopi", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Octopi { get; set; }

        [JsonProperty("_plugins", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Plugins { get; set; }

        [JsonProperty("_releases", NullValueHandling = NullValueHandling.Ignore)]
        public Blog Releases { get; set; }
    }
}
