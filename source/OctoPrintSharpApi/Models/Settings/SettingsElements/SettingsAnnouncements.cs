using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class SettingsAnnouncements
    {
        #region Properties
        [JsonProperty("channel_order", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ChannelOrder { get; set; } = new();

        [JsonProperty("channels", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsChannels Channels { get; set; }

        [JsonProperty("display_limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? DisplayLimit { get; set; }

        [JsonProperty("enabled_channels", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> EnabledChannels { get; set; } = new();

        [JsonProperty("forced_channels", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ForcedChannels { get; set; } = new(); 

        [JsonProperty("summary_limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? SummaryLimit { get; set; }

        [JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? Ttl { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
