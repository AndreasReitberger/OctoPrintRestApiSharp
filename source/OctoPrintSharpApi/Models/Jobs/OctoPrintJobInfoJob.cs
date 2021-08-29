using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintJobInfoJob
    {
        #region Properties
        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintJobInfoFile File { get; set; }

        [JsonProperty("estimatedPrintTime", NullValueHandling = NullValueHandling.Ignore)]
        public long EstimatedPrintTime { get; set; }

        [JsonProperty("filament", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintFilament Filament { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
