using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models.Printer
{
    public partial class OctoPrintPrinterCustomBox : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("x_max", NullValueHandling = NullValueHandling.Ignore)]
        long? xMax;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("x_min", NullValueHandling = NullValueHandling.Ignore)]
        long? xMin;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("y_max", NullValueHandling = NullValueHandling.Ignore)]
        long? yMax;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("y_min", NullValueHandling = NullValueHandling.Ignore)]
        long? yMin;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("z_max", NullValueHandling = NullValueHandling.Ignore)]
        long? zMax;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("z_min", NullValueHandling = NullValueHandling.Ignore)]
        long? zMin;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
