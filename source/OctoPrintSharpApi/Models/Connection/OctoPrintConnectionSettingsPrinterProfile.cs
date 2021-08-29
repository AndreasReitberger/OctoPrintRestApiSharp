using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintConnectionSettingsPrinterProfile
    {
        #region Properties
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
