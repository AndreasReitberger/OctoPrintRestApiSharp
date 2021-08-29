using Newtonsoft.Json;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class SettingsFolder
    {
        #region Properties
        [JsonProperty("logs", NullValueHandling = NullValueHandling.Ignore)]
        public string Logs { get; set; }

        [JsonProperty("timelapse", NullValueHandling = NullValueHandling.Ignore)]
        public string Timelapse { get; set; }

        [JsonProperty("timelapseTmp", NullValueHandling = NullValueHandling.Ignore)]
        public string TimelapseTmp { get; set; }

        [JsonProperty("uploads", NullValueHandling = NullValueHandling.Ignore)]
        public string Uploads { get; set; }

        [JsonProperty("watched", NullValueHandling = NullValueHandling.Ignore)]
        public string Watched { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
