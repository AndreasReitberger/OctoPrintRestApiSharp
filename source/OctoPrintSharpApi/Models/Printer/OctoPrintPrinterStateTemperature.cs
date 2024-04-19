using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateTemperature : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperatureInfo? bed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateTemperatureInfo? chamber;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        List<OctoPrintPrinterStateHistory> history = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateToolheadInfo? tool0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintPrinterStateToolheadInfo? tool1;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
