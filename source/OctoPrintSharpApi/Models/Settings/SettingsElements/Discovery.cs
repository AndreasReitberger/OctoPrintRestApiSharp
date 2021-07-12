using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class Discovery
    {
        [JsonProperty("httpPassword")]
        public object HttpPassword { get; set; }

        [JsonProperty("httpUsername")]
        public object HttpUsername { get; set; }

        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public Model Model { get; set; }

        [JsonProperty("pathPrefix")]
        public object PathPrefix { get; set; }

        [JsonProperty("publicHost")]
        public object PublicHost { get; set; }

        [JsonProperty("publicPort", NullValueHandling = NullValueHandling.Ignore)]
        public long? PublicPort { get; set; }

        [JsonProperty("upnpUuid", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? UpnpUuid { get; set; }

        [JsonProperty("zeroConf", NullValueHandling = NullValueHandling.Ignore)]
        public object[] ZeroConf { get; set; }
    }
}
