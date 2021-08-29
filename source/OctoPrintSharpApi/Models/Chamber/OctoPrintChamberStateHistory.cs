using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintChamberStateHistory
    {
        #region Properties
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public long? Time { get; set; }

        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Bed { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
