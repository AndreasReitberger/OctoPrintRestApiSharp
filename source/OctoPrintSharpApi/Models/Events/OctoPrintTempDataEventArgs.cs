using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class OctoPrintTempDataEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public object TemperatureData { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
