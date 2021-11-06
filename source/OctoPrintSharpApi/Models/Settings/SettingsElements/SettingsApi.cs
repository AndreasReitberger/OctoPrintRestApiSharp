using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class SettingsApi
    {
        #region Properties
        [JsonProperty("allowCrossOrigin", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowCrossOrigin { get; set; }

        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
