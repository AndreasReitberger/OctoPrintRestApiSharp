using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsEvents : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("commerror", NullValueHandling = NullValueHandling.Ignore)]
        bool? commerror;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("plugin", NullValueHandling = NullValueHandling.Ignore)]
        bool? plugin;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pong", NullValueHandling = NullValueHandling.Ignore)]
        bool? pong;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        bool? printer;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printer_safety_check", NullValueHandling = NullValueHandling.Ignore)]
        bool? printerSafetyCheck;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printjob", NullValueHandling = NullValueHandling.Ignore)]
        bool? printjob;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("slicing", NullValueHandling = NullValueHandling.Ignore)]
        bool? slicing;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("startup", NullValueHandling = NullValueHandling.Ignore)]
        bool? startup;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("throttled", NullValueHandling = NullValueHandling.Ignore)]
        bool? throttled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("update", NullValueHandling = NullValueHandling.Ignore)]
        bool? update;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
