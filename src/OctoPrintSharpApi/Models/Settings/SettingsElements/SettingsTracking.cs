using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsTracking : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Enabled { get; set; }

        [ObservableProperty]
        
        [JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsEvents? Events { get; set; }

        [ObservableProperty]
        
        [JsonProperty("ping")]
        public partial object? Ping { get; set; }

        [ObservableProperty]
        
        [JsonProperty("pong", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Pong { get; set; }

        [ObservableProperty]
        
        [JsonProperty("server")]
        public partial object? Server { get; set; }

        [ObservableProperty]
        
        [JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        public partial Guid? UniqueId { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
