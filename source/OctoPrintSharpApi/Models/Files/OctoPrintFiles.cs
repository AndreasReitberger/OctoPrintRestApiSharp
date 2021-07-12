using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFiles
    {
        [JsonProperty("files")]
        public OctoPrintFile[] Files { get; set; }


        [JsonProperty("children")]
        public OctoPrintFile[] Children { get; set; }

        [JsonProperty("free")]
        public long Free { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }
}
