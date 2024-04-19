using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsAppearance : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("closeModalsWithClick", NullValueHandling = NullValueHandling.Ignore)]
        bool? closeModalsWithClick;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        string color = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("colorIcon", NullValueHandling = NullValueHandling.Ignore)]
        bool? colorIcon;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("colorTransparent", NullValueHandling = NullValueHandling.Ignore)]
        bool? colorTransparent;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("defaultLanguage", NullValueHandling = NullValueHandling.Ignore)]
        string defaultLanguage = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("fuzzyTimes", NullValueHandling = NullValueHandling.Ignore)]
        bool? fuzzyTimes;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("showFahrenheitAlso", NullValueHandling = NullValueHandling.Ignore)]
        bool? showFahrenheitAlso;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
