using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsSystemClass : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
        List<object> actions = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("events")]
        object events;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
