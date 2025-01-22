using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFilePrintTime : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("_default")]
        public partial double DefaultValue { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
