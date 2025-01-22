using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterAxes : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("e")]
        public partial OctoPrintPrinterAxesAttribute? E { get; set; }

        [ObservableProperty]

        [JsonProperty("x")]
        public partial OctoPrintPrinterAxesAttribute? X { get; set; }

        [ObservableProperty]

        [JsonProperty("y")]
        public partial OctoPrintPrinterAxesAttribute? Y { get; set; }

        [ObservableProperty]

        [JsonProperty("z")]
        public partial OctoPrintPrinterAxesAttribute? Z { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
