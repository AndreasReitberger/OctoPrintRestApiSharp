using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsGcodeAnalysis : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("runAt", NullValueHandling = NullValueHandling.Ignore)]
        public partial string RunAt { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
