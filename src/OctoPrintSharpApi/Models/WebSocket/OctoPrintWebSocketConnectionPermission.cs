using AndreasReitberger.API.OctoPrint.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebSocketConnectionPermission : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("key")]
        public partial string Key { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("dangerous")]
        public partial bool Dangerous { get; set; }

        [ObservableProperty]

        [JsonProperty("default_groups")]
        public partial List<OctoPrintDefaultGroup> DefaultGroups { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("description")]
        public partial string Description { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("needs")]
        public partial OctoPrintWebSocketConnectionNeeds? Needs { get; set; }

        [ObservableProperty]

        [JsonProperty("plugin", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Plugin { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
