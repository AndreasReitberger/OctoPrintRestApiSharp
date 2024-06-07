using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintTempDataEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public object? TemperatureData;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
