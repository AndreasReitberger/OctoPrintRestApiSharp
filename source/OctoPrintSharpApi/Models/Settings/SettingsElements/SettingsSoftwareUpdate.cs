using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsSoftwareUpdate : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cache_ttl", NullValueHandling = NullValueHandling.Ignore)]
        long? cacheTtl;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ignore_throttled", NullValueHandling = NullValueHandling.Ignore)]
        bool? ignoreThrottled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("notify_users", NullValueHandling = NullValueHandling.Ignore)]
        bool? notifyUsers;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("octoprint_branch_mappings", NullValueHandling = NullValueHandling.Ignore)]
        SettingsOctoprintBranchMapping[] octoprintBranchMappings = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("octoprint_checkout_folder")]
        object? octoprintCheckoutFolder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("octoprint_method", NullValueHandling = NullValueHandling.Ignore)]
        string octoprintMethod = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("octoprint_pip_target", NullValueHandling = NullValueHandling.Ignore)]
        string octoprintPipTarget = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("octoprint_release_channel", NullValueHandling = NullValueHandling.Ignore)]
        string octoprintReleaseChannel = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("octoprint_tracked_branch")]
        object? octoprintTrackedBranch;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("octoprint_type", NullValueHandling = NullValueHandling.Ignore)]
        string octoprintType = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pip_command")]
        object? pipCommand;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pip_enable_check", NullValueHandling = NullValueHandling.Ignore)]
        bool? pipEnableCheck;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
