using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileStatistics : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("averagePrintTime")]
        OctoPrintFilePrintTime? averagePrintTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("lastPrintTime")]
        OctoPrintFilePrintTime? lastPrintTime;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
