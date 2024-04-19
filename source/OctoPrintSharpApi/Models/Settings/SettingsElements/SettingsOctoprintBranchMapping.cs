using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsOctoprintBranchMapping : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("branch", NullValueHandling = NullValueHandling.Ignore)]
        string branch = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("commitish", NullValueHandling = NullValueHandling.Ignore)]
        List<string> commitish = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        string name = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
