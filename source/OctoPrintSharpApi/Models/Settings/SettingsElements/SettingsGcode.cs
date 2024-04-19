using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsGcode : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("afterPrintCancelled", NullValueHandling = NullValueHandling.Ignore)]
        string afterPrintCancelled = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("snippets/disable_bed", NullValueHandling = NullValueHandling.Ignore)]
        string snippetsDisableBed = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("snippets/disable_hotends", NullValueHandling = NullValueHandling.Ignore)]
        string snippetsDisableHotends = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
