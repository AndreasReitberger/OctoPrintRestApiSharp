using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsDiscovery : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("httpPassword")]
        public partial object? HttpPassword { get; set; }

        [ObservableProperty]
        
        [JsonProperty("httpUsername")]
        public partial object? HttpUsername { get; set; }

        [ObservableProperty]
        
        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsModel? Model { get; set; }

        [ObservableProperty]
        
        [JsonProperty("pathPrefix")]
        public partial object? PathPrefix { get; set; }

        [ObservableProperty]
        
        [JsonProperty("publicHost")]
        public partial object? PublicHost { get; set; }

        [ObservableProperty]
        
        [JsonProperty("publicPort", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? PublicPort { get; set; }

        [ObservableProperty]
        
        [JsonProperty("upnpUuid", NullValueHandling = NullValueHandling.Ignore)]
        public partial Guid? UpnpUuid { get; set; }

        [ObservableProperty]
        
        [JsonProperty("zeroConf", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<object> ZeroConf { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
