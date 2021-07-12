using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintJobInfoFile
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public long Size { get; set; }

        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public long Date { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }
    }
}
