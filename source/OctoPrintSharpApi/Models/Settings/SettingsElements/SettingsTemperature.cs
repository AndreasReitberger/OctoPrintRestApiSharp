using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class SettingsTemperature
    {
        #region Properties
        [JsonProperty("cutoff", NullValueHandling = NullValueHandling.Ignore)]
        public long? Cutoff { get; set; }

        [JsonProperty("profiles", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsProfile[] Profiles { get; set; }

        [JsonProperty("sendAutomatically", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SendAutomatically { get; set; }

        [JsonProperty("sendAutomaticallyAfter", NullValueHandling = NullValueHandling.Ignore)]
        public long? SendAutomaticallyAfter { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
