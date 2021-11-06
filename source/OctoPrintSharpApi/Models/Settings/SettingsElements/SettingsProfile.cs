using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class SettingsProfile
    {
        #region Properties
        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public long? Bed { get; set; }

        [JsonProperty("chamber")]
        public object Chamber { get; set; }

        [JsonProperty("extruder", NullValueHandling = NullValueHandling.Ignore)]
        public long? Extruder { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
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
