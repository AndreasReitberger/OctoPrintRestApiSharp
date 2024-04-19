using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterVolume : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("depth")]
        long depth;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("formFactor")]
        string formFactor = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("height")]
        long height;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("origin")]
        string origin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("width")]
        long width;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
