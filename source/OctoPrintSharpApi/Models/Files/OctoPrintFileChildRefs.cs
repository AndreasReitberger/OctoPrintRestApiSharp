using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileChildRefs
    {
        [JsonProperty("download")]
        public Uri Download { get; set; }

        [JsonProperty("resource")]
        public Uri Resource { get; set; }
    }
}
