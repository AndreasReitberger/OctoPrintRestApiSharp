using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsOctoprintBranchMapping
    {
        #region Properties
        [JsonProperty("branch", NullValueHandling = NullValueHandling.Ignore)]
        public string Branch { get; set; }

        [JsonProperty("commitish", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Commitish { get; set; } = new();

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
