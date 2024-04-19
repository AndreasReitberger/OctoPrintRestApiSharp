using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileDimensions : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("depth")]
        double depth;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("height")]
        double height;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("width")]
        double width;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
