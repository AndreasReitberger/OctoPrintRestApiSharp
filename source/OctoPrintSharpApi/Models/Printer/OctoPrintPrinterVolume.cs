using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterVolume
    {
        #region Properties
        [JsonProperty("depth")]
        public long Depth { get; set; }

        [JsonProperty("formFactor")]
        public string FormFactor { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
