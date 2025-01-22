using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintJobInfoJob : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintJobInfoFile? File { get; set; }

        [ObservableProperty]

        [JsonProperty("estimatedPrintTime", NullValueHandling = NullValueHandling.Ignore)]
        public partial long EstimatedPrintTime { get; set; }

        [ObservableProperty]

        [JsonProperty("filament", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintFilament? Filament { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
