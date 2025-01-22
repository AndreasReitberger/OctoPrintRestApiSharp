using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileDimensions : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("depth")]
        public partial double Depth { get; set; }

        [ObservableProperty]
        
        [JsonProperty("height")]
        public partial double Height { get; set; }

        [ObservableProperty]
        
        [JsonProperty("width")]
        public partial double Width { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
