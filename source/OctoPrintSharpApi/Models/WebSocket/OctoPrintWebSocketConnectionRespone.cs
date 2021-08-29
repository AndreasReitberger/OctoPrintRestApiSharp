using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintWebSocketConnectionRespone
    {
        #region Properties
        [JsonProperty("connected")]
        public OctoPrintWebSocketConnectionInfo Connected { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
