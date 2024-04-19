using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsDiskSpace : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("critical", NullValueHandling = NullValueHandling.Ignore)]
        long? critical;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("warning", NullValueHandling = NullValueHandling.Ignore)]
        long? warning;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
