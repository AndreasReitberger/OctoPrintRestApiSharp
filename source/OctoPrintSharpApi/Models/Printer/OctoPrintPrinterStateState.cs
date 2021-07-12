using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterStateState
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, bool> Flags { get; set; }
    }
}
