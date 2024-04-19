using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileLastPrint : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("date")]
        double date;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printTime")]
        double printTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("success")]
        bool success;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
