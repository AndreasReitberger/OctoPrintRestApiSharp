using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateSd : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("ready", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? Ready { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
