using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintJobInfoProgress
    {
        #region Properties
        [JsonProperty("completion", NullValueHandling = NullValueHandling.Ignore)]
        public double Completion { get; set; }

        [JsonProperty("filepos", NullValueHandling = NullValueHandling.Ignore)]
        public long Filepos { get; set; }

        [JsonProperty("printTime", NullValueHandling = NullValueHandling.Ignore)]
        public long PrintTime { get; set; }

        [JsonProperty("printTimeLeft", NullValueHandling = NullValueHandling.Ignore)]
        public long PrintTimeLeft { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
