using AndreasReitberger.API.OctoPrint.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebSocketConnectionPermission
    {
        #region Properties
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
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
