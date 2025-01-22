using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintConnectionSettingsConnection : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("baudrate", NullValueHandling = NullValueHandling.Ignore)]
        public partial long Baudrate { get; set; }

        [ObservableProperty]

        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Port { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("printerProfile", NullValueHandling = NullValueHandling.Ignore)]
        public partial string PrinterProfile { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public partial string State { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
