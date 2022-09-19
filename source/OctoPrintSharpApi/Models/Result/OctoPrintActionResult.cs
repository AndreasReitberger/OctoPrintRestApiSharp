using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintActionResult
    {
        #region Properties
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
