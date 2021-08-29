using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintJobInfo
    {
        #region Properties
        [JsonProperty("job")]
        public OctoPrintJobInfoJob Job { get; set; }

        [JsonProperty("progress")]
        public OctoPrintJobInfoProgress Progress { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }

}
