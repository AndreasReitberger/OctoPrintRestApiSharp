using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsTemperature : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cutoff", NullValueHandling = NullValueHandling.Ignore)]
        long? cutoff;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("profiles", NullValueHandling = NullValueHandling.Ignore)]
        SettingsProfile[] profiles = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sendAutomatically", NullValueHandling = NullValueHandling.Ignore)]
        bool? sendAutomatically;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sendAutomaticallyAfter", NullValueHandling = NullValueHandling.Ignore)]
        long? sendAutomaticallyAfter;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
