using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsGcode
    {
        #region Properties
        [JsonProperty("afterPrintCancelled", NullValueHandling = NullValueHandling.Ignore)]
        public string AfterPrintCancelled { get; set; }

        [JsonProperty("snippets/disable_bed", NullValueHandling = NullValueHandling.Ignore)]
        public string SnippetsDisableBed { get; set; }

        [JsonProperty("snippets/disable_hotends", NullValueHandling = NullValueHandling.Ignore)]
        public string SnippetsDisableHotends { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
