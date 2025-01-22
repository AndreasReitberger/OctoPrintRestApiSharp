using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebSocketConnectionInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("version")]
        public partial string Version { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("display_version")]
        public partial string DisplayVersion { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("branch")]
        public partial string Branch { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("plugin_hash")]
        public partial string PluginHash { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("config_hash")]
        public partial string ConfigHash { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("debug")]
        public partial bool Debug { get; set; }

        [ObservableProperty]
        
        [JsonProperty("safe_mode")]
        public partial object? SafeMode { get; set; }

        [ObservableProperty]
        
        [JsonProperty("permissions")]
        public partial List<OctoPrintWebSocketConnectionPermission> Permissions { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
