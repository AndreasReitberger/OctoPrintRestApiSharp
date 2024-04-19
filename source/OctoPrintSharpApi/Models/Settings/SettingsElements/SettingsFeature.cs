using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsFeature : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("autoUppercaseBlacklist", NullValueHandling = NullValueHandling.Ignore)]
        List<string> autoUppercaseBlacklist = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("g90InfluencesExtruder", NullValueHandling = NullValueHandling.Ignore)]
        bool? g90InfluencesExtruder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcodeViewer", NullValueHandling = NullValueHandling.Ignore)]
        bool? gcodeViewer;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("keyboardControl", NullValueHandling = NullValueHandling.Ignore)]
        bool? keyboardControl;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mobileSizeThreshold", NullValueHandling = NullValueHandling.Ignore)]
        long? mobileSizeThreshold;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("modelSizeDetection", NullValueHandling = NullValueHandling.Ignore)]
        bool? modelSizeDetection;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pollWatched", NullValueHandling = NullValueHandling.Ignore)]
        bool? pollWatched;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printCancelConfirmation", NullValueHandling = NullValueHandling.Ignore)]
        bool? printCancelConfirmation;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printStartConfirmation", NullValueHandling = NullValueHandling.Ignore)]
        bool? printStartConfirmation;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sdSupport", NullValueHandling = NullValueHandling.Ignore)]
        bool? sdSupport;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sizeThreshold", NullValueHandling = NullValueHandling.Ignore)]
        long? sizeThreshold;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("temperatureGraph", NullValueHandling = NullValueHandling.Ignore)]
        bool? temperatureGraph;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
