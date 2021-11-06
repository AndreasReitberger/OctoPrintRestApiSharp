using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class SettingsDiskSpace
    {
        #region Properties
        [JsonProperty("critical", NullValueHandling = NullValueHandling.Ignore)]
        public long? Critical { get; set; }

        [JsonProperty("warning", NullValueHandling = NullValueHandling.Ignore)]
        public long? Warning { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
