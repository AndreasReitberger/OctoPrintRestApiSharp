using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models.Settings.SettingsElements
{
    public partial class Appearance
    {
        [JsonProperty("closeModalsWithClick", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CloseModalsWithClick { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("colorIcon", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ColorIcon { get; set; }

        [JsonProperty("colorTransparent", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ColorTransparent { get; set; }

        [JsonProperty("defaultLanguage", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultLanguage { get; set; }

        [JsonProperty("fuzzyTimes", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FuzzyTimes { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("showFahrenheitAlso", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowFahrenheitAlso { get; set; }
    }
}
