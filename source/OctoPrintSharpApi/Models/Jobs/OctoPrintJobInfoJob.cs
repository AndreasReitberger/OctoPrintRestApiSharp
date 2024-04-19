using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintJobInfoJob : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintJobInfoFile? file;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("estimatedPrintTime", NullValueHandling = NullValueHandling.Ignore)]
        long estimatedPrintTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filament", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintFilament? filament;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
