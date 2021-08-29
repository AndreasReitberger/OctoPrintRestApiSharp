using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterAxes
    {
        #region Properties
        [JsonProperty("e")]
        public OctoPrintPrinterAxesAttribute E { get; set; }

        [JsonProperty("x")]
        public OctoPrintPrinterAxesAttribute X { get; set; }

        [JsonProperty("y")]
        public OctoPrintPrinterAxesAttribute Y { get; set; }

        [JsonProperty("z")]
        public OctoPrintPrinterAxesAttribute Z { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
