using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintWebSocketConnectionRespone
    {
        [JsonProperty("connected")]
        public OctoPrintWebSocketConnectionInfo Connected { get; set; }
    }
}
