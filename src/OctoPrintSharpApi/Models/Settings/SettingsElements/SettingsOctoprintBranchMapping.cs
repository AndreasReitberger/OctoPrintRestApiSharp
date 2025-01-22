using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsOctoprintBranchMapping : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("branch", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Branch { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("commitish", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<string> Commitish { get; set; } = new();

        [ObservableProperty]

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Name { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
