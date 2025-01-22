using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsSoftwareUpdate : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("cache_ttl", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? CacheTtl { get; set; }

        [ObservableProperty]

        [JsonProperty("ignore_throttled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? IgnoreThrottled { get; set; }

        [ObservableProperty]

        [JsonProperty("notify_users", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? NotifyUsers { get; set; }

        [ObservableProperty]

        [JsonProperty("octoprint_branch_mappings", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsOctoprintBranchMapping[] OctoprintBranchMappings { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("octoprint_checkout_folder")]
        public partial object? OctoprintCheckoutFolder { get; set; }

        [ObservableProperty]

        [JsonProperty("octoprint_method", NullValueHandling = NullValueHandling.Ignore)]
        public partial string OctoprintMethod { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("octoprint_pip_target", NullValueHandling = NullValueHandling.Ignore)]
        public partial string OctoprintPipTarget { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("octoprint_release_channel", NullValueHandling = NullValueHandling.Ignore)]
        public partial string OctoprintReleaseChannel { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("octoprint_tracked_branch")]
        public partial object? OctoprintTrackedBranch { get; set; }

        [ObservableProperty]

        [JsonProperty("octoprint_type", NullValueHandling = NullValueHandling.Ignore)]
        public partial string OctoprintType { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("pip_command")]
        public partial object? PipCommand { get; set; }

        [ObservableProperty]

        [JsonProperty("pip_enable_check", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? PipEnableCheck { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
