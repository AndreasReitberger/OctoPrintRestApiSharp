using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models
{
    public partial class SettingsServer
    {
        #region Properties
        [JsonProperty("allowFraming", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowFraming { get; set; }

        [JsonProperty("commands", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsCommands Commands { get; set; }

        [JsonProperty("diskspace", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsDiskSpace Diskspace { get; set; }

        [JsonProperty("onlineCheck", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsOnlineCheck OnlineCheck { get; set; }

        [JsonProperty("pluginBlacklist", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsPluginBlacklist PluginBlacklist { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
