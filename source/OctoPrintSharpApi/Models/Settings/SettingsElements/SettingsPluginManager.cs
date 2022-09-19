using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPluginManager
    {
        #region Properties
        [JsonProperty("confirm_disable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ConfirmDisable { get; set; }

        [JsonProperty("dependency_links", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DependencyLinks { get; set; }

        [JsonProperty("hidden", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Hidden { get; set; } = new();

        [JsonProperty("ignore_throttled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IgnoreThrottled { get; set; }

        [JsonProperty("notices", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Notices { get; set; }

        [JsonProperty("notices_ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? NoticesTtl { get; set; }

        [JsonProperty("pip_args")]
        public object PipArgs { get; set; }

        [JsonProperty("pip_force_user", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PipForceUser { get; set; }

        [JsonProperty("repository", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Repository { get; set; }

        [JsonProperty("repository_ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? RepositoryTtl { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
