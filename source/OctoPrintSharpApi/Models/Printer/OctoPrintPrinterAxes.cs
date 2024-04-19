using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterAxes : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("e")]
        OctoPrintPrinterAxesAttribute? e;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("x")]
        OctoPrintPrinterAxesAttribute? x;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("y")]
        OctoPrintPrinterAxesAttribute? y;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("z")]
        OctoPrintPrinterAxesAttribute? z;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
