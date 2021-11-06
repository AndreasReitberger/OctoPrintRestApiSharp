using AndreasReitberger.Enum;
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

        #region Static
        public static OctoPrintJobInfo Default = new()
        {
            State = OctoPrintConnectionStates.Operational.ToString(),
            Progress = new OctoPrintJobInfoProgress() { Completion = 0 },
            Job = new OctoPrintJobInfoJob(),
        };
        #endregion

        #region Overrides
    public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }

}
