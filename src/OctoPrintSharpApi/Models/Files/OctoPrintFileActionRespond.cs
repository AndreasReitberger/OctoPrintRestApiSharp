using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintFileActionRespond : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("origin", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Origin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Path { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("refs", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoPrintFileActionRespondRefs? Refs { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
