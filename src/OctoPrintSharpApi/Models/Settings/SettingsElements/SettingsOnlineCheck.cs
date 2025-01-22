using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsOnlineCheck : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Enabled { get; set; }

        [ObservableProperty]
        
        [JsonProperty("host", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Host { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("interval", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Interval { get; set; }

        [ObservableProperty]
        
        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Port { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
