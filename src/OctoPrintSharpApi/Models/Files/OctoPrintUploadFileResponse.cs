using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintUploadFileResponse : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("done", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool Done { get; set; }

        [ObservableProperty]
        
        [JsonProperty("files", NullValueHandling = NullValueHandling.Ignore)]
        public partial Dictionary<string, OctoPrintFile> Files { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
