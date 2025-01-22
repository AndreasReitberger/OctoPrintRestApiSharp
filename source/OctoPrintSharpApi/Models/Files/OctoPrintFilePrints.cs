using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFilePrints : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("failure")]
        public partial long Failure { get; set; }

        [ObservableProperty]
        
        [JsonProperty("last")]
        public partial OctoPrintFileLastPrint? Last { get; set; }

        [ObservableProperty]
        
        [JsonProperty("success")]
        public partial long Success { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
