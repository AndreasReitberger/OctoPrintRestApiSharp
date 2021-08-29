using AndreasReitberger.Enum;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public partial class OctoPrintPrinterStateTemperatureInfo
    {
        #region Properties

        [JsonProperty("actual", NullValueHandling = NullValueHandling.Ignore)]
        public double Actual { get; set; }

        [JsonProperty("target")]
        public long Target { get; set; }

        [JsonProperty("offset", NullValueHandling = NullValueHandling.Ignore)]
        public long Offset { get; set; }

        [JsonIgnore]
        public OctoPrintCurrentToolState State { get => GetCurrentState(); }

        #endregion

        #region Methods
        OctoPrintCurrentToolState GetCurrentState()
        {
            if (Target < 0 || Actual < 0)
                return OctoPrintCurrentToolState.Error;
            else
            {
                if (Target <= 0)
                    return OctoPrintCurrentToolState.Idle;
                // Check if temperature is reached with a hysteresis
                else if (Target > Actual && Math.Abs(Target - Actual) > 2)
                    return OctoPrintCurrentToolState.Heating;
                else
                    return OctoPrintCurrentToolState.Ready;
            }
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
