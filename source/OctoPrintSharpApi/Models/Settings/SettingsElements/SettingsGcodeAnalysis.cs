using Newtonsoft.Json;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class SettingsGcodeAnalysis
    {
        #region Properties
        [JsonProperty("runAt", NullValueHandling = NullValueHandling.Ignore)]
        public string RunAt { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
