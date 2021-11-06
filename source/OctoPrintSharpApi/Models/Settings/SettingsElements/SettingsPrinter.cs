using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class SettingsPrinter
    {
        #region Properties
        [JsonProperty("defaultExtrusionLength", NullValueHandling = NullValueHandling.Ignore)]
        public long? DefaultExtrusionLength { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
