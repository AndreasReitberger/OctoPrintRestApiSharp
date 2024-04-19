using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintUploadFileResponse : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("done", NullValueHandling = NullValueHandling.Ignore)]
        bool done;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("files", NullValueHandling = NullValueHandling.Ignore)]
        Dictionary<string, OctoPrintFile> files = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
