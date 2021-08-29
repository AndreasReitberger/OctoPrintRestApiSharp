using Newtonsoft.Json;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class SettingsCommands
    {
        #region Properties
        [JsonProperty("serverRestartCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerRestartCommand { get; set; }

        [JsonProperty("systemRestartCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string SystemRestartCommand { get; set; }

        [JsonProperty("systemShutdownCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string SystemShutdownCommand { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
