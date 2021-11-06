using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class SettingsEvents
    {
        #region Properties
        [JsonProperty("commerror", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Commerror { get; set; }

        [JsonProperty("plugin", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Plugin { get; set; }

        [JsonProperty("pong", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Pong { get; set; }

        [JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Printer { get; set; }

        [JsonProperty("printer_safety_check", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PrinterSafetyCheck { get; set; }

        [JsonProperty("printjob", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Printjob { get; set; }

        [JsonProperty("slicing", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Slicing { get; set; }

        [JsonProperty("startup", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Startup { get; set; }

        [JsonProperty("throttled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Throttled { get; set; }

        [JsonProperty("update", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Update { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
