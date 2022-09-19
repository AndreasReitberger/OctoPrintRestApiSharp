using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintUploadFileRespone
    {
        #region Properties
        [JsonProperty("done", NullValueHandling = NullValueHandling.Ignore)]
        public bool Done { get; set; }

        [JsonProperty("files", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, OctoPrintFile> Files { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
