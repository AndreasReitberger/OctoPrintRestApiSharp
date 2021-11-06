using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class SettingsScripts
    {
        #region Properties
        [JsonProperty("gcode", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Gcode { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
