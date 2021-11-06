using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintSettings
    {
        #region Properties
        [JsonProperty("api", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsApi Api { get; set; }

        [JsonProperty("appearance", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsAppearance Appearance { get; set; }

        [JsonProperty("feature", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsFeature Feature { get; set; }

        [JsonProperty("folder", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsFolder Folder { get; set; }

        [JsonProperty("gcodeAnalysis", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsGcodeAnalysis GcodeAnalysis { get; set; }

        [JsonProperty("plugins", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsPlugins Plugins { get; set; }

        [JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsPrinter Printer { get; set; }

        [JsonProperty("scripts", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsScripts Scripts { get; set; }

        [JsonProperty("serial", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsSerial Serial { get; set; }

        [JsonProperty("server", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsServer Server { get; set; }

        [JsonProperty("system", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsSystemClass System { get; set; }

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsTemperature Temperature { get; set; }

        [JsonProperty("terminalFilters", NullValueHandling = NullValueHandling.Ignore)]
        public List<SettingsTerminalFilter> TerminalFilters { get; set; }

        [JsonProperty("webcam", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsWebcam Webcam { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
