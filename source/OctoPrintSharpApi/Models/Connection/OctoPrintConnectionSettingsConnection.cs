using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintConnectionSettingsConnection
    {
        #region Properties
        [JsonProperty("baudrate", NullValueHandling = NullValueHandling.Ignore)]
        public long Baudrate { get; set; }

        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public string Port { get; set; }

        [JsonProperty("printerProfile", NullValueHandling = NullValueHandling.Ignore)]
        public string PrinterProfile { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
