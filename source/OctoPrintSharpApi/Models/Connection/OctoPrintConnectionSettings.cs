using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintConnectionSettings
    {
        #region Properties
        [JsonProperty("current")]
        public OctoPrintConnectionSettingsConnection Current { get; set; }

        [JsonProperty("options")]
        public OctoPrintConnectionSettingsOptions Options { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
