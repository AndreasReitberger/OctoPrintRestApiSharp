using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintChamberState
    {
        #region Properties
        [JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Chamber { get; set; }

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public List<OctoPrintChamberStateHistory> History { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
