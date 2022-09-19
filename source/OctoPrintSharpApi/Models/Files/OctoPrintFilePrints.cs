using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFilePrints
    {
        #region Properties
        [JsonProperty("failure")]
        public long Failure { get; set; }

        [JsonProperty("last")]
        public OctoPrintFileLastPrint Last { get; set; }

        [JsonProperty("success")]
        public long Success { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
