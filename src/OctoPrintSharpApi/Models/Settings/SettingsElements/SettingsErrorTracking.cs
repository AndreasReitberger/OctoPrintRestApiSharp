using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsErrorTracking : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Enabled { get; set; }

        [ObservableProperty]

        [JsonProperty("enabled_unreleased", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? EnabledUnreleased { get; set; }

        [ObservableProperty]

        [JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        public partial Guid? UniqueId { get; set; }

        [ObservableProperty]

        [JsonProperty("url_coreui", NullValueHandling = NullValueHandling.Ignore)]
        public partial Uri? UrlCoreui { get; set; }

        [ObservableProperty]

        [JsonProperty("url_server", NullValueHandling = NullValueHandling.Ignore)]
        public partial Uri? UrlServer { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
