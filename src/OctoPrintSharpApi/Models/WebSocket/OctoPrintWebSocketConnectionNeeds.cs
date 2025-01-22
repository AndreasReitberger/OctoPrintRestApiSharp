using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintWebSocketConnectionNeeds : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("role")]
        public partial List<string> Role { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }

}
