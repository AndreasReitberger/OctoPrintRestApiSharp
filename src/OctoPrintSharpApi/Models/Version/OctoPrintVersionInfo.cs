using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintVersionInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("api")]
        public partial string Api { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("server")]
        public partial string Server { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("text")]
        public partial string Text { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
