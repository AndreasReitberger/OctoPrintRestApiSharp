using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsGcode : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("afterPrintCancelled", NullValueHandling = NullValueHandling.Ignore)]
        public partial string AfterPrintCancelled { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("snippets/disable_bed", NullValueHandling = NullValueHandling.Ignore)]
        public partial string SnippetsDisableBed { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("snippets/disable_hotends", NullValueHandling = NullValueHandling.Ignore)]
        public partial string SnippetsDisableHotends { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
