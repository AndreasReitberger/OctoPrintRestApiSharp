using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsModel : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("description")]
        public partial object? Description { get; set; }

        [ObservableProperty]

        [JsonProperty("name")]
        public partial object? Ame { get; set; }

        [ObservableProperty]

        [JsonProperty("number")]
        public partial object? Number { get; set; }

        [ObservableProperty]

        [JsonProperty("serial")]
        public partial object? Serial { get; set; }

        [ObservableProperty]

        [JsonProperty("url")]
        public partial object? Url { get; set; }

        [ObservableProperty]

        [JsonProperty("vendor")]
        public partial object? Vendor { get; set; }

        [ObservableProperty]

        [JsonProperty("vendorUrl")]
        public partial object? VendorUrl { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
