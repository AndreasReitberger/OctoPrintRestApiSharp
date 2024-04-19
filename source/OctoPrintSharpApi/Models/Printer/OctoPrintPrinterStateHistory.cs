using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperatureInfo? bed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        long? time;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperatureInfo? tool0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperatureInfo? tool1;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
