using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsCommands : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("serverRestartCommand", NullValueHandling = NullValueHandling.Ignore)]
        string serverRestartCommand = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("systemRestartCommand", NullValueHandling = NullValueHandling.Ignore)]
        string systemRestartCommand = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("systemShutdownCommand", NullValueHandling = NullValueHandling.Ignore)]
        string systemShutdownCommand = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
