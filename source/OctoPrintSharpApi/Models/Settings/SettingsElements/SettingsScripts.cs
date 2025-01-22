using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsScripts : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("gcode", NullValueHandling = NullValueHandling.Ignore)]
        public partial Dictionary<string, string> Gcode { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
