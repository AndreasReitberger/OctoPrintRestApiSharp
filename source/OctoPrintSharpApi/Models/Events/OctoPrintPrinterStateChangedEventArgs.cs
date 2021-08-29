
using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class OctoPrintPrinterStateChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public OctoPrintPrinterState NewPrinterState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
