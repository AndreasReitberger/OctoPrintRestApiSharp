using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterStateTemperature
    {
        #region Properties
        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Bed { get; set; }

        [JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Chamber { get; set; }

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateHistory[] History { get; set; }

        [JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Tool0 { get; set; }

        [JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Tool1 { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
