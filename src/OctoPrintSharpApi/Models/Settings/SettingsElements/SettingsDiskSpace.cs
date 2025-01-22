using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsDiskSpace : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("critical", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Critical { get; set; }

        [ObservableProperty]

        [JsonProperty("warning", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Warning { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
