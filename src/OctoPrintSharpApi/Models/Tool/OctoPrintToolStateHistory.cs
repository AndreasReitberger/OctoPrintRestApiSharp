using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintToolStateHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Time { get; set; }

        [ObservableProperty]

        [JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperatureInfo? Tool0 { get; set; }

        [ObservableProperty]

        [JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperatureInfo? Tool1 { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
