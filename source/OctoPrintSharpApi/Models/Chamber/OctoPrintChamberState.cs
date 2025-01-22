using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintChamberState : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperatureInfo? Chamber { get; set; }

        [ObservableProperty]
        
        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<OctoPrintChamberStateHistory> History { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
