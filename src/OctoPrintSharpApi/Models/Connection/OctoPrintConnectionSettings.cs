using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintConnectionSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("current")]
        public partial OctoPrintConnectionSettingsConnection? Current { get; set; }

        [ObservableProperty]

        [JsonProperty("options")]
        public partial OctoPrintConnectionSettingsOptions? Options { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
