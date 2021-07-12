using System;
using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileActionRespondRefs
    {
        [JsonProperty("download", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Download { get; set; }

        [JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Resource { get; set; }
    }
}
