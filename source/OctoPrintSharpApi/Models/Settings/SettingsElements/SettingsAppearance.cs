using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsAppearance : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("closeModalsWithClick", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? CloseModalsWithClick { get; set; }

        [ObservableProperty]
        
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Color { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("colorIcon", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? ColorIcon { get; set; }

        [ObservableProperty]
        
        [JsonProperty("colorTransparent", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? ColorTransparent { get; set; }

        [ObservableProperty]
        
        [JsonProperty("defaultLanguage", NullValueHandling = NullValueHandling.Ignore)]
        public partial string DefaultLanguage { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("fuzzyTimes", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? FuzzyTimes { get; set; }

        [ObservableProperty]
        
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("showFahrenheitAlso", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? ShowFahrenheitAlso { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
