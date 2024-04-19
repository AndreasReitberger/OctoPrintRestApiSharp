using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterState : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperature? temperature;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sd", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateSd? sd;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateState? state;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
