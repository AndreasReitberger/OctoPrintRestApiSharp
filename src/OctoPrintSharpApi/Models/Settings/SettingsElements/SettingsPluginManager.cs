using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPluginManager : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("confirm_disable", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? ConfirmDisable { get; set; }

        [ObservableProperty]
        
        [JsonProperty("dependency_links", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? DependencyLinks { get; set; }

        [ObservableProperty]
        
        [JsonProperty("hidden", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<object> Hidden { get; set; } = new();

        [ObservableProperty]
        
        [JsonProperty("ignore_throttled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? IgnoreThrottled { get; set; }

        [ObservableProperty]
        
        [JsonProperty("notices", NullValueHandling = NullValueHandling.Ignore)]
        public partial Uri? Notices { get; set; }

        [ObservableProperty]
        
        [JsonProperty("notices_ttl", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? NoticesTtl { get; set; }

        [ObservableProperty]
        
        [JsonProperty("pip_args")]
        public partial object? PipArgs { get; set; }

        [ObservableProperty]
        
        [JsonProperty("pip_force_user", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? PipForceUser { get; set; }

        [ObservableProperty]
        
        [JsonProperty("repository", NullValueHandling = NullValueHandling.Ignore)]
        public partial Uri? Repository { get; set; }

        [ObservableProperty]
        
        [JsonProperty("repository_ttl", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? RepositoryTtl { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
