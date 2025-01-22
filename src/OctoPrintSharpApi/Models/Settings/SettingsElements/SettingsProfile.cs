using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsProfile : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Bed { get; set; }

        [ObservableProperty]

        [JsonProperty("chamber")]
        public partial object? Chamber { get; set; }

        [ObservableProperty]

        [JsonProperty("extruder", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Extruder { get; set; }

        [ObservableProperty]

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Name { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
