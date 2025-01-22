using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models.Printer
{
    public partial class OctoPrintPrinterCustomBox : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("x_max", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? XMax { get; set; }

        [ObservableProperty]

        [JsonProperty("x_min", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? XMin { get; set; }

        [ObservableProperty]

        [JsonProperty("y_max", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? YMax { get; set; }

        [ObservableProperty]

        [JsonProperty("y_min", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? YMin { get; set; }

        [ObservableProperty]

        [JsonProperty("z_max", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? ZMax { get; set; }

        [ObservableProperty]

        [JsonProperty("z_min", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? ZMin { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
