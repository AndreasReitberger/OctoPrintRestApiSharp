using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintActionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("ok")]
        public partial bool Ok { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
