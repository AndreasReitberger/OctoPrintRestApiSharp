using Newtonsoft.Json;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class SettingsSoftwareUpdate
    {
        #region Properties
        [JsonProperty("cache_ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? CacheTtl { get; set; }

        [JsonProperty("ignore_throttled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IgnoreThrottled { get; set; }

        [JsonProperty("notify_users", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NotifyUsers { get; set; }

        [JsonProperty("octoprint_branch_mappings", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsOctoprintBranchMapping[] OctoprintBranchMappings { get; set; }

        [JsonProperty("octoprint_checkout_folder")]
        public object OctoprintCheckoutFolder { get; set; }

        [JsonProperty("octoprint_method", NullValueHandling = NullValueHandling.Ignore)]
        public string OctoprintMethod { get; set; }

        [JsonProperty("octoprint_pip_target", NullValueHandling = NullValueHandling.Ignore)]
        public string OctoprintPipTarget { get; set; }

        [JsonProperty("octoprint_release_channel", NullValueHandling = NullValueHandling.Ignore)]
        public string OctoprintReleaseChannel { get; set; }

        [JsonProperty("octoprint_tracked_branch")]
        public object OctoprintTrackedBranch { get; set; }

        [JsonProperty("octoprint_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OctoprintType { get; set; }

        [JsonProperty("pip_command")]
        public object PipCommand { get; set; }

        [JsonProperty("pip_enable_check", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PipEnableCheck { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
