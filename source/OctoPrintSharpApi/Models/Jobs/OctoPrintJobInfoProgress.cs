using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintJobInfoProgress : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("completion", NullValueHandling = NullValueHandling.Ignore)]
        double completion;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filepos", NullValueHandling = NullValueHandling.Ignore)]
        long filepos;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printTime", NullValueHandling = NullValueHandling.Ignore)]
        long printTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printTimeLeft", NullValueHandling = NullValueHandling.Ignore)]
        long printTimeLeft;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
