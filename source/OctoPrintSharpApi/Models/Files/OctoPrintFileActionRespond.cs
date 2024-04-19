using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileActionRespond : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("origin", NullValueHandling = NullValueHandling.Ignore)]
        string origin;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        string name;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        string path;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("refs", NullValueHandling = NullValueHandling.Ignore)]
        OctoPrintFileActionRespondRefs? refs;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
