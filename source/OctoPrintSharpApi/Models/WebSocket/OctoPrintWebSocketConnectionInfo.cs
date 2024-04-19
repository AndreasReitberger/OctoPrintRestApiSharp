using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebSocketConnectionInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("version")]
        string version = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("display_version")]
        string displayVersion = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("branch")]
        string branch = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("plugin_hash")]
        string pluginHash = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("config_hash")]
        string configHash = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("debug")]
        bool debug;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("safe_mode")]
        object? safeMode;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("permissions")]
        List<OctoPrintWebSocketConnectionPermission> permissions = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
