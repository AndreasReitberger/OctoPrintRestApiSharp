using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterState
    {
        #region Properties
        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperature Temperature { get; set; }

        [JsonProperty("sd", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateSd Sd { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateState State { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
