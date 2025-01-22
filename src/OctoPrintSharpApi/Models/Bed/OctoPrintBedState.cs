using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintBedState : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperatureInfo? Bed { get; set; }

        [ObservableProperty]
        
        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<OctoPrintBedStateHistory> History { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
