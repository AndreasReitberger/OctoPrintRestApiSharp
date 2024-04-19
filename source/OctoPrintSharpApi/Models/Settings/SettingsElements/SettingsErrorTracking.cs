using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsErrorTracking : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        bool? enabled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enabled_unreleased", NullValueHandling = NullValueHandling.Ignore)]
        bool? enabledUnreleased;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        Guid? uniqueId;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("url_coreui", NullValueHandling = NullValueHandling.Ignore)]
        Uri? urlCoreui;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("url_server", NullValueHandling = NullValueHandling.Ignore)]
        Uri? urlServer;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
