using AndreasReitberger.API.OctoPrint.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebSocketConnectionPermission : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("key")]
        string key = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("dangerous")]
        bool dangerous;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("default_groups")]
        List<OctoPrintDefaultGroup> defaultGroups = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("description")]
        string description = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("needs")]
        OctoPrintWebSocketConnectionNeeds? needs;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("plugin", NullValueHandling = NullValueHandling.Ignore)]
        string plugin = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
