using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsAnnouncements : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("channel_order", NullValueHandling = NullValueHandling.Ignore)]
        List<string> channelOrder = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("channels", NullValueHandling = NullValueHandling.Ignore)]
        SettingsChannels? channels;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("display_limit", NullValueHandling = NullValueHandling.Ignore)]
        long? displayLimit;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enabled_channels", NullValueHandling = NullValueHandling.Ignore)]
        List<string> enabledChannels = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("forced_channels", NullValueHandling = NullValueHandling.Ignore)]
        List<string> forcedChannels = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("summary_limit", NullValueHandling = NullValueHandling.Ignore)]
        long? summaryLimit;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        long? ttl;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
