using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsOnlineCheck : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        bool? enabled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("host", NullValueHandling = NullValueHandling.Ignore)]
        string host = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("interval", NullValueHandling = NullValueHandling.Ignore)]
        long? interval;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        long? port;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
