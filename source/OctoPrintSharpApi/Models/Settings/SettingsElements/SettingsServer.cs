using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsServer : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("allowFraming", NullValueHandling = NullValueHandling.Ignore)]
        bool? allowFraming;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("commands", NullValueHandling = NullValueHandling.Ignore)]
        SettingsCommands? commands;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("diskspace", NullValueHandling = NullValueHandling.Ignore)]
        SettingsDiskSpace? diskspace;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("onlineCheck", NullValueHandling = NullValueHandling.Ignore)]
        SettingsOnlineCheck? onlineCheck;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pluginBlacklist", NullValueHandling = NullValueHandling.Ignore)]
        SettingsPluginBlacklist? pluginBlacklist;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
