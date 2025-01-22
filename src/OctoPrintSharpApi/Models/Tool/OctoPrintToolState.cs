using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintToolState : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperatureInfo? Tool0 { get; set; }

        [ObservableProperty]

        [JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperatureInfo? Tool1 { get; set; }

        [ObservableProperty]

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<OctoPrintToolStateHistory> History { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
