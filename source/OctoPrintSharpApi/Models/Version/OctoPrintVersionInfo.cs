using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintVersionInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("api")]
        string api = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("server")]
        string server = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("text")]
        string text = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
