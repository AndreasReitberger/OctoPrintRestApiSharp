using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterState : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperature? Temperature { get; set; }

        [ObservableProperty]

        [JsonProperty("sd", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateSd? Sd { get; set; }

        [ObservableProperty]

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateState? State { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
