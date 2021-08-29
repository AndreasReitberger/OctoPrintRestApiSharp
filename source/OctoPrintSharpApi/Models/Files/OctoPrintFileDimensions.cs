using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFileDimensions
    {
        #region Properties
        [JsonProperty("depth")]
        public double Depth { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("width")]
        public double Width { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
