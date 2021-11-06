using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public partial class SettingsBlog
    {
        #region Properties
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        public long? Priority { get; set; }

        [JsonProperty("read_until", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReadUntil { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

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
