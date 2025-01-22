using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsFolder : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("logs", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Logs { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("timelapse", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Timelapse { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("timelapseTmp", NullValueHandling = NullValueHandling.Ignore)]
        public partial string TimelapseTmp { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("uploads", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Uploads { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("watched", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Watched { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
