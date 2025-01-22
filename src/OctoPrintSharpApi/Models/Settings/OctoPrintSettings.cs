using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("api", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsApi? Api { get; set; }

        [ObservableProperty]

        [JsonProperty("appearance", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsAppearance? Appearance { get; set; }

        [ObservableProperty]

        [JsonProperty("feature", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsFeature? Feature { get; set; }

        [ObservableProperty]

        [JsonProperty("folder", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsFolder? Folder { get; set; }

        [ObservableProperty]

        [JsonProperty("gcodeAnalysis", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsGcodeAnalysis? GcodeAnalysis { get; set; }

        [ObservableProperty]

        [JsonProperty("plugins", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsPlugins? Plugins { get; set; }

        [ObservableProperty]

        [JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsPrinter? Printer { get; set; }

        [ObservableProperty]

        [JsonProperty("scripts", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsScripts? Scripts { get; set; }

        [ObservableProperty]

        [JsonProperty("serial", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsSerial? Serial { get; set; }

        [ObservableProperty]

        [JsonProperty("server", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsServer? Server { get; set; }

        [ObservableProperty]

        [JsonProperty("system", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsSystemClass? System { get; set; }

        [ObservableProperty]

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsTemperature? Temperature { get; set; }

        [ObservableProperty]

        [JsonProperty("terminalFilters", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<SettingsTerminalFilter> TerminalFilters { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("webcam", NullValueHandling = NullValueHandling.Ignore)]
        public partial SettingsWebcam? Webcam { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
