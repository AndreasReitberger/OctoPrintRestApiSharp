using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileLastPrint : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("date")]
        public partial double Date { get; set; }

        [ObservableProperty]
        
        [JsonProperty("printTime")]
        public partial double PrintTime { get; set; }

        [ObservableProperty]
        
        [JsonProperty("success")]
        public partial bool Success { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
