using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsChannels : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("_blog", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsBlog? Blog { get; set; }

        [ObservableProperty]

        [JsonProperty("_important", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsBlog? Important { get; set; }

        [ObservableProperty]

        [JsonProperty("_octopi", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsBlog? Octopi { get; set; }

        [ObservableProperty]

        [JsonProperty("_plugins", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsBlog? Plugins { get; set; }

        [ObservableProperty]

        [JsonProperty("_releases", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsBlog? Releases { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
