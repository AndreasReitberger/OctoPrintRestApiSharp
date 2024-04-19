using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPlugins : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("action_command_prompt", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsActionCommandPrompt? actionCommandPrompt;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("announcements", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsAnnouncements? announcements;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("discovery", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsDiscovery? discovery;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("errortracking", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsErrorTracking? errortracking;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pi_support", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsPiSupport? piSupport;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pluginmanager", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsPluginManager? pluginmanager;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("slic3r", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsSlic3R? slic3R;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("softwareupdate", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsSoftwareUpdate? softwareupdate;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tracking", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsTracking? tracking;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
