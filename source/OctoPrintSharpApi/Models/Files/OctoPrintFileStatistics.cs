using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileStatistics
    {
        [JsonProperty("averagePrintTime")]
        public OctoPrintFilePrintTime AveragePrintTime { get; set; }

        [JsonProperty("lastPrintTime")]
        public OctoPrintFilePrintTime LastPrintTime { get; set; }
    }
}
