using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsCommands : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("serverRestartCommand", NullValueHandling = NullValueHandling.Ignore)]
        public partial string ServerRestartCommand { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("systemRestartCommand", NullValueHandling = NullValueHandling.Ignore)]
        public partial string SystemRestartCommand { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("systemShutdownCommand", NullValueHandling = NullValueHandling.Ignore)]
        public partial string SystemShutdownCommand { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
