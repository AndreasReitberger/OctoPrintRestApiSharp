using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintPrinterConfigChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public OctoPrintSettings? NewConfiguration;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
