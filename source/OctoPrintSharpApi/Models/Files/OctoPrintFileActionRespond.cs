using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileActionRespond
    {
        [JsonProperty("origin", NullValueHandling = NullValueHandling.Ignore)]
        public string Origin { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("refs", NullValueHandling = NullValueHandling.Ignore)]
        public OctoPrintFileActionRespondRefs Refs { get; set; }
    }
}
