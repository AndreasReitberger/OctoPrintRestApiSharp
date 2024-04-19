using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsDiscovery : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("httpPassword")]
        object? httpPassword;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("httpUsername")]
        object? httpUsername;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        SettingsModel? model;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pathPrefix")]
        object? pathPrefix;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("publicHost")]
        object? publicHost;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("publicPort", NullValueHandling = NullValueHandling.Ignore)]
        long? publicPort;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("upnpUuid", NullValueHandling = NullValueHandling.Ignore)]
        Guid? upnpUuid;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("zeroConf", NullValueHandling = NullValueHandling.Ignore)]
        List<object> zeroConf = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
