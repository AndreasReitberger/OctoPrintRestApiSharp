using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPluginManager : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("confirm_disable", NullValueHandling = NullValueHandling.Ignore)]
        bool? confirmDisable;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("dependency_links", NullValueHandling = NullValueHandling.Ignore)]
        bool? dependencyLinks;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("hidden", NullValueHandling = NullValueHandling.Ignore)]
        List<object> hidden = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ignore_throttled", NullValueHandling = NullValueHandling.Ignore)]
        bool? ignoreThrottled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("notices", NullValueHandling = NullValueHandling.Ignore)]
        Uri? notices;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("notices_ttl", NullValueHandling = NullValueHandling.Ignore)]
        long? noticesTtl;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pip_args")]
        object? pipArgs;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pip_force_user", NullValueHandling = NullValueHandling.Ignore)]
        bool? pipForceUser;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("repository", NullValueHandling = NullValueHandling.Ignore)]
        Uri? repository;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("repository_ttl", NullValueHandling = NullValueHandling.Ignore)]
        long? repositoryTtl;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
