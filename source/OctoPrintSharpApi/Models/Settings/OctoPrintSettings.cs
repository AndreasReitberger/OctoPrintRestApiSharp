using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("api", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsApi? api;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("appearance", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsAppearance? appearance;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("feature", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsFeature? feature;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("folder", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsFolder? folder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcodeAnalysis", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsGcodeAnalysis? gcodeAnalysis;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("plugins", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsPlugins? plugins;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsPrinter? printer;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("scripts", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsScripts? scripts;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("serial", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsSerial? serial;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("server", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsServer? server;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("system", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsSystemClass? system;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsTemperature? temperature;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("terminalFilters", NullValueHandling = NullValueHandling.Ignore)]
        public List<SettingsTerminalFilter> terminalFilters = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("webcam", NullValueHandling = NullValueHandling.Ignore)]
        public SettingsWebcam? webcam;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
