using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFilament : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
        double length;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("volume", NullValueHandling = NullValueHandling.Ignore)]
        double volume;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
