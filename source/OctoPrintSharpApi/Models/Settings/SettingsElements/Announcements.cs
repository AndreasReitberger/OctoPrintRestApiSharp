using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class Announcements
    {
        [JsonProperty("channel_order", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ChannelOrder { get; set; }

        [JsonProperty("channels", NullValueHandling = NullValueHandling.Ignore)]
        public Channels Channels { get; set; }

        [JsonProperty("display_limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? DisplayLimit { get; set; }

        [JsonProperty("enabled_channels", NullValueHandling = NullValueHandling.Ignore)]
        public string[] EnabledChannels { get; set; }

        [JsonProperty("forced_channels", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ForcedChannels { get; set; }

        [JsonProperty("summary_limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? SummaryLimit { get; set; }

        [JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? Ttl { get; set; }
    }
}
