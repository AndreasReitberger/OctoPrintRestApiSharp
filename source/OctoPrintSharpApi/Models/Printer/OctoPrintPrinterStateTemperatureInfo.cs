using AndreasReitberger.API.OctoPrint.Enum;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public partial class OctoPrintPrinterStateTemperatureInfo
    {
        #region Properties

        [JsonProperty("actual", NullValueHandling = NullValueHandling.Ignore)]
        public double? Actual { get; set; } = 0;

        [JsonProperty("target")]
        public double? Target { get; set; } = 0;

        [JsonProperty("offset", NullValueHandling = NullValueHandling.Ignore)]
        public double? Offset { get; set; } = 0;

        [JsonIgnore]
        public OctoPrintCurrentToolState State { get => GetCurrentState(); }

        #endregion

        #region Static
        public static OctoPrintPrinterStateTemperatureInfo Default = new()
        {
            Actual = 0,
            Target = 0,
            Offset = 0,
        };
        #endregion

        #region Methods
        OctoPrintCurrentToolState GetCurrentState()
        {
            if (Target == null || Target < 0 || Actual == null || Actual < 0)
                return OctoPrintCurrentToolState.Error;
            else
            {
                if (Target <= 0)
                    return OctoPrintCurrentToolState.Idle;
                // Check if temperature is reached with a hysteresis
                else if (Target > Actual && Math.Abs((double)Target - (double)Actual) > 2)
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
