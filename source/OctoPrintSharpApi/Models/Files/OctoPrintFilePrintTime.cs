using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintFilePrintTime
    {
        #region Properties
        [JsonProperty("_default")]
        public double Default { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
