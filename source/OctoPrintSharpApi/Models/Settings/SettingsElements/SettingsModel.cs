using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class SettingsModel
    {
        #region Properties
        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("name")]
        public object Name { get; set; }

        [JsonProperty("number")]
        public object Number { get; set; }

        [JsonProperty("serial")]
        public object Serial { get; set; }

        [JsonProperty("url")]
        public object Url { get; set; }

        [JsonProperty("vendor")]
        public object Vendor { get; set; }

        [JsonProperty("vendorUrl")]
        public object VendorUrl { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
