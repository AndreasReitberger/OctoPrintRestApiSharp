using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPluginBlacklist : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        bool? enabled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        long? ttl;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        Uri? url;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
