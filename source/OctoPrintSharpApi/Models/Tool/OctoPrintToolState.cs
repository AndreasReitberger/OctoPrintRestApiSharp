using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintToolState
    {
        #region Properties
        [JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Tool0 { get; set; }

        [JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintPrinterStateTemperatureInfo Tool1 { get; set; }

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public List<OctoPrintToolStateHistory> History { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
