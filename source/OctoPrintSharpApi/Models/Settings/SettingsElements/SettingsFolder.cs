using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsFolder : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("logs", NullValueHandling = NullValueHandling.Ignore)]
        string logs = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timelapse", NullValueHandling = NullValueHandling.Ignore)]
        string timelapse = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timelapseTmp", NullValueHandling = NullValueHandling.Ignore)]
        string timelapseTmp = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("uploads", NullValueHandling = NullValueHandling.Ignore)]
        string uploads = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("watched", NullValueHandling = NullValueHandling.Ignore)]
        string watched = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
