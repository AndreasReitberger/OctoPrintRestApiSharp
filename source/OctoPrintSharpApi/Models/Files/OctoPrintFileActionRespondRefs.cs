using System;
using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileActionRespondRefs : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("download", NullValueHandling = NullValueHandling.Ignore)]
        Uri? download;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
        Uri? resource;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
