using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintUploadFileRespone
    {
        [JsonProperty("done", NullValueHandling = NullValueHandling.Ignore)]
        public bool Done { get; set; }

        [JsonProperty("files", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, OctoPrintFile> Files { get; set; }
    }
}
