using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsServer : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("allowFraming", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? AllowFraming { get; set; }

        [ObservableProperty]

        [JsonProperty("commands", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsCommands? Commands { get; set; }

        [ObservableProperty]

        [JsonProperty("diskspace", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsDiskSpace? Diskspace { get; set; }

        [ObservableProperty]

        [JsonProperty("onlineCheck", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsOnlineCheck? OnlineCheck { get; set; }

        [ObservableProperty]

        [JsonProperty("pluginBlacklist", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsPluginBlacklist? PluginBlacklist { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
