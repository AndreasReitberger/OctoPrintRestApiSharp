using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsAnnouncements : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("channel_order", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<string> ChannelOrder { get; set; } = new();

        [ObservableProperty]
        
        [JsonProperty("channels", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsChannels? Channels { get; set; }

        [ObservableProperty]
        
        [JsonProperty("display_limit", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? DisplayLimit { get; set; }

        [ObservableProperty]
        
        [JsonProperty("enabled_channels", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<string> EnabledChannels { get; set; } = new();

        [ObservableProperty]
        
        [JsonProperty("forced_channels", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<string> ForcedChannels { get; set; } = new();

        [ObservableProperty]
        
        [JsonProperty("summary_limit", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? SummaryLimit { get; set; }

        [ObservableProperty]
        
        [JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Ttl { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
