using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsPrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("defaultExtrusionLength", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? DefaultExtrusionLength { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
