using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintChamberState : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperatureInfo? chamber;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        List<OctoPrintChamberStateHistory> history = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
