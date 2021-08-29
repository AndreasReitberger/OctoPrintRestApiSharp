using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class SettingsErrorTracking
    {
        #region Properties
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("enabled_unreleased", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnabledUnreleased { get; set; }

        [JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? UniqueId { get; set; }

        [JsonProperty("url_coreui", NullValueHandling = NullValueHandling.Ignore)]
        public Uri UrlCoreui { get; set; }

        [JsonProperty("url_server", NullValueHandling = NullValueHandling.Ignore)]
        public Uri UrlServer { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
