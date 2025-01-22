using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintBedStateHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Time { get; set; }

        [ObservableProperty]

        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperatureInfo? Bed { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
