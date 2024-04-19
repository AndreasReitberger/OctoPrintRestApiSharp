using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintConnectionSettingsConnection : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("baudrate", NullValueHandling = NullValueHandling.Ignore)]
        long baudrate;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        string port = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printerProfile", NullValueHandling = NullValueHandling.Ignore)]
        string printerProfile = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        string state = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
