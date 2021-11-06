using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class SettingsSystemClass
    {
        #region Properties
        [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Actions { get; set; } = new();

        [JsonProperty("events")]
        public object Events { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
