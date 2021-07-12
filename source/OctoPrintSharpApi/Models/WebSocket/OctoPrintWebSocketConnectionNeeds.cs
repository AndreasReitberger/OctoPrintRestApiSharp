using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintWebSocketConnectionNeeds
    {
        [JsonProperty("role")]
        public List<string> Role { get; set; }
    }

}
