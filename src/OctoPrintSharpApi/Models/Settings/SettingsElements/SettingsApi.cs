using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsApi : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("allowCrossOrigin", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? AllowCrossOrigin { get; set; }

        [ObservableProperty]
        
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Key { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
