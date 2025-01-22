using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsEvents : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("commerror", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Commerror { get; set; }

        [ObservableProperty]

        [JsonProperty("plugin", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Plugin { get; set; }

        [ObservableProperty]

        [JsonProperty("pong", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Pong { get; set; }

        [ObservableProperty]

        [JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Printer { get; set; }

        [ObservableProperty]

        [JsonProperty("printer_safety_check", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? PrinterSafetyCheck { get; set; }

        [ObservableProperty]

        [JsonProperty("printjob", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Printjob { get; set; }

        [ObservableProperty]

        [JsonProperty("slicing", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Slicing { get; set; }

        [ObservableProperty]

        [JsonProperty("startup", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Startup { get; set; }

        [ObservableProperty]

        [JsonProperty("throttled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Throttled { get; set; }

        [ObservableProperty]

        [JsonProperty("update", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Update { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
