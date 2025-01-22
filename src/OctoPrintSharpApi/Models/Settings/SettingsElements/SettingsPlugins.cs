using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPlugins : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("action_command_prompt", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsActionCommandPrompt? ActionCommandPrompt { get; set; }

        [ObservableProperty]

        [JsonProperty("announcements", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsAnnouncements? Announcements { get; set; }

        [ObservableProperty]

        [JsonProperty("discovery", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsDiscovery? Discovery { get; set; }

        [ObservableProperty]

        [JsonProperty("errortracking", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsErrorTracking? Errortracking { get; set; }

        [ObservableProperty]

        [JsonProperty("pi_support", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsPiSupport? PiSupport { get; set; }

        [ObservableProperty]

        [JsonProperty("pluginmanager", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsPluginManager? Pluginmanager { get; set; }

        [ObservableProperty]

        [JsonProperty("slic3r", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsSlic3R? Slic3R { get; set; }

        [ObservableProperty]

        [JsonProperty("softwareupdate", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsSoftwareUpdate? Softwareupdate { get; set; }

        [ObservableProperty]

        [JsonProperty("tracking", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsTracking? Tracking { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
