using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterProfiles
    {
        #region Properties
        [JsonProperty("profiles")]
        public Dictionary<string, OctoPrintPrinter> Profiles { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }

}
