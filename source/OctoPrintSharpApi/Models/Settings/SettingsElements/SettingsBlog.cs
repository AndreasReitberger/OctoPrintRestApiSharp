using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsBlog : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        string description = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        long? priority;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("read_until", NullValueHandling = NullValueHandling.Ignore)]
        long? readUntil;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        string type = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        Uri? url;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
