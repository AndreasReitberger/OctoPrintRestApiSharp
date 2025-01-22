using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsTemperature : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("cutoff", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Cutoff { get; set; }

        [ObservableProperty]
        
        [JsonProperty("profiles", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsProfile[] Profiles { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("sendAutomatically", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? SendAutomatically { get; set; }

        [ObservableProperty]
        
        [JsonProperty("sendAutomaticallyAfter", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? SendAutomaticallyAfter { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
