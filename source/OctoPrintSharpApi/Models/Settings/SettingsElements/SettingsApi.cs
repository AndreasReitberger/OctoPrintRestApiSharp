using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsApi : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("allowCrossOrigin", NullValueHandling = NullValueHandling.Ignore)]
        bool? allowCrossOrigin;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        string key = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
