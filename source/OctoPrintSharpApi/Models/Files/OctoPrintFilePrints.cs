using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFilePrints : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("failure")]
        long failure;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("last")]
        OctoPrintFileLastPrint? last;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("success")]
        long success;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
