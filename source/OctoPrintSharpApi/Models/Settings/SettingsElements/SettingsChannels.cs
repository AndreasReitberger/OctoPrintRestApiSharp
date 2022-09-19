using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsChannels
    {
        #region Properties
        [JsonProperty("_blog", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsBlog Blog { get; set; }

        [JsonProperty("_important", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsBlog Important { get; set; }

        [JsonProperty("_octopi", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsBlog Octopi { get; set; }

        [JsonProperty("_plugins", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsBlog Plugins { get; set; }

        [JsonProperty("_releases", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsBlog Releases { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
