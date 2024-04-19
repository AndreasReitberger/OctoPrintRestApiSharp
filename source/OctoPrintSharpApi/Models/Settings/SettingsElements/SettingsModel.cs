using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsModel : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("description")]
        object? description;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        object? ame;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("number")]
        object? number;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("serial")]
        object? serial;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("url")]
        object? url;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("vendor")]
        object? vendor;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("vendorUrl")]
        object? vendorUrl;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
