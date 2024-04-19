using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("count")]
        long count;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("nozzleDiameter")]
        double nozzleDiameter;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("offsets")]
        long[][] offsets = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sharedNozzle")]
        bool sharedNozzle;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
