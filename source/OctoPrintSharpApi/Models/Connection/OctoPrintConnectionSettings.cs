using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintConnectionSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("current")]
        OctoPrintConnectionSettingsConnection? current;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("options")]
        OctoPrintConnectionSettingsOptions? options;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
