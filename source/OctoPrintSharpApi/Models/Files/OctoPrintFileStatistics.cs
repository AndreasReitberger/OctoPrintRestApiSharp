using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileStatistics
    {
        #region Properties
        [JsonProperty("averagePrintTime")]
        public OctoPrintFilePrintTime AveragePrintTime { get; set; }

        [JsonProperty("lastPrintTime")]
        public OctoPrintFilePrintTime LastPrintTime { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
