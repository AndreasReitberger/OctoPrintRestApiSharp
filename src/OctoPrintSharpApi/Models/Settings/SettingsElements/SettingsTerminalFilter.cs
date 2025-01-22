using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsTerminalFilter : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("regex", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Regex { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
