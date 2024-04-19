using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsProfile : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        long? bed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("chamber")]
        object? chamber;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder", NullValueHandling = NullValueHandling.Ignore)]
        long? extruder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        string name = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
