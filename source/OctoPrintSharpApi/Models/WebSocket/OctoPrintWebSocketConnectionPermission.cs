using AndreasReitberger.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintWebSocketConnectionPermission
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dangerous")]
        public bool Dangerous { get; set; }

        [JsonProperty("default_groups")]
        public List<OctoPrintDefaultGroup> DefaultGroups { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("needs")]
        public OctoPrintWebSocketConnectionNeeds Needs { get; set; }

        [JsonProperty("plugin", NullValueHandling = NullValueHandling.Ignore)]
        public string Plugin { get; set; }
    }
}
