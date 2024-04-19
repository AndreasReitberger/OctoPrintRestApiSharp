using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintToolState : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperatureInfo? tool0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperatureInfo? tool1;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        List<OctoPrintToolStateHistory> history = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
