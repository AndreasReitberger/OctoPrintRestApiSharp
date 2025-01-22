using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterAxesAttribute : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("inverted")]
        public partial bool Inverted { get; set; }

        [ObservableProperty]

        [JsonProperty("speed")]
        public partial long Speed { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
