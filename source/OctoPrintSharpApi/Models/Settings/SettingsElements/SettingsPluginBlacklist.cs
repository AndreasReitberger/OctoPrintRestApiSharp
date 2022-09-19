using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPluginBlacklist
    {
        #region Properties
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? Ttl { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
