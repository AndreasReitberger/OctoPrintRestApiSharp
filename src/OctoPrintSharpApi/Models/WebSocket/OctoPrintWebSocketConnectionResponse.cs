using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebSocketConnectionResponse : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("connected")]
        public partial OctoPrintWebSocketConnectionInfo? Connected { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
