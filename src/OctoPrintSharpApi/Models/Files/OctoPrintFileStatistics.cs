using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileStatistics : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("averagePrintTime")]
        public partial OctoPrintFilePrintTime? AveragePrintTime { get; set; }

        [ObservableProperty]

        [JsonProperty("lastPrintTime")]
        public partial OctoPrintFilePrintTime? LastPrintTime { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
