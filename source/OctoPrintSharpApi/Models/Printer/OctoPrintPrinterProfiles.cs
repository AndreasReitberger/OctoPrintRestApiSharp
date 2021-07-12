using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterProfiles
    {
        [JsonProperty("profiles")]
        public Dictionary<string, OctoPrintPrinter> Profiles { get; set; }
    }

}
