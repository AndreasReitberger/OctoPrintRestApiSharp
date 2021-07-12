using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class Model
    {
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
    }
}
