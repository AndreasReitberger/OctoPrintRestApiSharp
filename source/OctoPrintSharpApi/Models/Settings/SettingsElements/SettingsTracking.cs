using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsTracking
    {
        #region Properties
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsEvents Events { get; set; }

        [JsonProperty("ping")]
        public object Ping { get; set; }

        [JsonProperty("pong", NullValueHandling = NullValueHandling.Ignore)]
        public long? Pong { get; set; }

        [JsonProperty("server")]
        public object Server { get; set; }

        [JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? UniqueId { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
