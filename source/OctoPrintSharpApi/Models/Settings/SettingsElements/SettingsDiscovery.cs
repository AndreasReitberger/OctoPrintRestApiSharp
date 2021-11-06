using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class SettingsDiscovery
    {
        #region Properties
        [JsonProperty("httpPassword")]
        public object HttpPassword { get; set; }

        [JsonProperty("httpUsername")]
        public object HttpUsername { get; set; }

        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsModel Model { get; set; }

        [JsonProperty("pathPrefix")]
        public object PathPrefix { get; set; }

        [JsonProperty("publicHost")]
        public object PublicHost { get; set; }

        [JsonProperty("publicPort", NullValueHandling = NullValueHandling.Ignore)]
        public long? PublicPort { get; set; }

        [JsonProperty("upnpUuid", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? UpnpUuid { get; set; }

        [JsonProperty("zeroConf", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> ZeroConf { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
