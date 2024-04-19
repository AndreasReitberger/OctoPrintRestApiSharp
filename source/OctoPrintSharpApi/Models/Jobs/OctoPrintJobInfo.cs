using AndreasReitberger.API.OctoPrint.Enum;
using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintJobInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("job")]
        OctoPrintJobInfoJob? job;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("progress")]
        OctoPrintJobInfoProgress? progress;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state")]
        string state = string.Empty;
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
