using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class SettingsFeature
    {
        #region Properties
        [JsonProperty("autoUppercaseBlacklist", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AutoUppercaseBlacklist { get; set; } = new();

        [JsonProperty("g90InfluencesExtruder", NullValueHandling = NullValueHandling.Ignore)]
        public bool? G90InfluencesExtruder { get; set; }

        [JsonProperty("gcodeViewer", NullValueHandling = NullValueHandling.Ignore)]
        public bool? GcodeViewer { get; set; }

        [JsonProperty("keyboardControl", NullValueHandling = NullValueHandling.Ignore)]
        public bool? KeyboardControl { get; set; }

        [JsonProperty("mobileSizeThreshold", NullValueHandling = NullValueHandling.Ignore)]
        public long? MobileSizeThreshold { get; set; }

        [JsonProperty("modelSizeDetection", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ModelSizeDetection { get; set; }

        [JsonProperty("pollWatched", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PollWatched { get; set; }

        [JsonProperty("printCancelConfirmation", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PrintCancelConfirmation { get; set; }

        [JsonProperty("printStartConfirmation", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PrintStartConfirmation { get; set; }

        [JsonProperty("sdSupport", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SdSupport { get; set; }

        [JsonProperty("sizeThreshold", NullValueHandling = NullValueHandling.Ignore)]
        public long? SizeThreshold { get; set; }

        [JsonProperty("temperatureGraph", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TemperatureGraph { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
