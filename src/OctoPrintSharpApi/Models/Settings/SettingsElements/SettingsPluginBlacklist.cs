using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPluginBlacklist : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Enabled { get; set; }

        [ObservableProperty]

        [JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Ttl { get; set; }

        [ObservableProperty]

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public partial Uri? Url { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
