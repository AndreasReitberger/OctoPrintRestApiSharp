using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models.Printer
{
    public partial class OctoPrintPrinterCustomBox
    {
        #region Properties
        [JsonProperty("x_max", NullValueHandling = NullValueHandling.Ignore)]
        public long? XMax { get; set; }

        [JsonProperty("x_min", NullValueHandling = NullValueHandling.Ignore)]
        public long? XMin { get; set; }

        [JsonProperty("y_max", NullValueHandling = NullValueHandling.Ignore)]
        public long? YMax { get; set; }

        [JsonProperty("y_min", NullValueHandling = NullValueHandling.Ignore)]
        public long? YMin { get; set; }

        [JsonProperty("z_max", NullValueHandling = NullValueHandling.Ignore)]
        public long? ZMax { get; set; }

        [JsonProperty("z_min", NullValueHandling = NullValueHandling.Ignore)]
        public long? ZMin { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
