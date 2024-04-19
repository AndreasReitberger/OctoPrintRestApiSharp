using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintBedStateHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        long? time;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperatureInfo? bed;

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
