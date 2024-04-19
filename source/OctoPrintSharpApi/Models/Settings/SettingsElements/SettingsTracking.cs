using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsTracking : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        bool? enabled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
        SettingsEvents? events;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ping")]
        object? ping;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pong", NullValueHandling = NullValueHandling.Ignore)]
        long? pong;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("server")]
        object? server;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        Guid? uniqueId;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
