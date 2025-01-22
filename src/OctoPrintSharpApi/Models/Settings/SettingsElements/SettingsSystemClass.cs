using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsSystemClass : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<object> Actions { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("events")]
        public partial object? Events { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
