using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPlugins
    {
        #region Properties
        [JsonProperty("action_command_prompt", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsActionCommandPrompt ActionCommandPrompt { get; set; }

        [JsonProperty("announcements", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsAnnouncements Announcements { get; set; }

        [JsonProperty("discovery", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsDiscovery Discovery { get; set; }

        [JsonProperty("errortracking", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsErrorTracking Errortracking { get; set; }

        [JsonProperty("pi_support", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsPiSupport PiSupport { get; set; }

        [JsonProperty("pluginmanager", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsPluginManager Pluginmanager { get; set; }

        [JsonProperty("slic3r", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsSlic3R Slic3R { get; set; }

        [JsonProperty("softwareupdate", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsSoftwareUpdate Softwareupdate { get; set; }

        [JsonProperty("tracking", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsTracking Tracking { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
