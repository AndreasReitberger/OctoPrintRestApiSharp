using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintJobInfoProgress : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("completion", NullValueHandling = NullValueHandling.Ignore)]
        public partial double Completion { get; set; }

        [ObservableProperty]

        [JsonProperty("filepos", NullValueHandling = NullValueHandling.Ignore)]
        public partial long Filepos { get; set; }

        [ObservableProperty]

        [JsonProperty("printTime", NullValueHandling = NullValueHandling.Ignore)]
        public partial long PrintTime { get; set; }

        [ObservableProperty]

        [JsonProperty("printTimeLeft", NullValueHandling = NullValueHandling.Ignore)]
        public partial long PrintTimeLeft { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
