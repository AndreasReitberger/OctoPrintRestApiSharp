using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintJobInfoFile : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("origin")]
        public partial string Origin { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public partial long Size { get; set; }

        [ObservableProperty]
        
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public partial long Date { get; set; }

        [ObservableProperty]
        
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Path { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
