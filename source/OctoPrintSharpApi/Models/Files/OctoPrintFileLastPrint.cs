using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileLastPrint
    {
        #region Properties
        [JsonProperty("date")]
        public double Date { get; set; }

        [JsonProperty("printTime")]
        public double PrintTime { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
