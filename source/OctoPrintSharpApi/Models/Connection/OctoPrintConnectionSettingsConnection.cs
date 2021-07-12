using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintConnectionSettingsConnection
    {
        [JsonProperty("baudrate", NullValueHandling = NullValueHandling.Ignore)]
        public long Baudrate { get; set; }

        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public string Port { get; set; }

        [JsonProperty("printerProfile", NullValueHandling = NullValueHandling.Ignore)]
        public string PrinterProfile { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
    }
}
