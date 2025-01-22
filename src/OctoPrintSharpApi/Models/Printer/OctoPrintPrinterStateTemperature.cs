using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateTemperature : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperatureInfo? Bed { get; set; }

        [ObservableProperty]

        [JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateTemperatureInfo? Chamber { get; set; }

        [ObservableProperty]

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<OctoPrintPrinterStateHistory> History { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateToolheadInfo? Tool0 { get; set; }

        [ObservableProperty]

        [JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintPrinterStateToolheadInfo? Tool1 { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
