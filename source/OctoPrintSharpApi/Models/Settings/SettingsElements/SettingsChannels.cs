using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsChannels : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("_blog", NullValueHandling = NullValueHandling.Ignore)]
        SettingsBlog? blog;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("_important", NullValueHandling = NullValueHandling.Ignore)]
        SettingsBlog? important;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("_octopi", NullValueHandling = NullValueHandling.Ignore)]
        SettingsBlog? octopi;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("_plugins", NullValueHandling = NullValueHandling.Ignore)]
        SettingsBlog? plugins;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("_releases", NullValueHandling = NullValueHandling.Ignore)]
        SettingsBlog? releases;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
