using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsFeature : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("autoUppercaseBlacklist", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<string> AutoUppercaseBlacklist { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("g90InfluencesExtruder", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? G90InfluencesExtruder { get; set; }

        [ObservableProperty]
        
        [JsonProperty("gcodeViewer", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? GcodeViewer { get; set; }

        [ObservableProperty]
        
        [JsonProperty("keyboardControl", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? KeyboardControl { get; set; }

        [ObservableProperty]
        
        [JsonProperty("mobileSizeThreshold", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? MobileSizeThreshold { get; set; }

        [ObservableProperty]
        
        [JsonProperty("modelSizeDetection", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? ModelSizeDetection { get; set; }

        [ObservableProperty]
        
        [JsonProperty("pollWatched", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? PollWatched { get; set; }

        [ObservableProperty]
        
        [JsonProperty("printCancelConfirmation", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? PrintCancelConfirmation { get; set; }

        [ObservableProperty]
        
        [JsonProperty("printStartConfirmation", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? PrintStartConfirmation { get; set; }

        [ObservableProperty]
        
        [JsonProperty("sdSupport", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? SdSupport { get; set; }

        [ObservableProperty]
        
        [JsonProperty("sizeThreshold", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? SizeThreshold { get; set; }

        [ObservableProperty]
        
        [JsonProperty("temperatureGraph", NullValueHandling = NullValueHandling.Ignore)]
        public partial bool? TemperatureGraph { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
