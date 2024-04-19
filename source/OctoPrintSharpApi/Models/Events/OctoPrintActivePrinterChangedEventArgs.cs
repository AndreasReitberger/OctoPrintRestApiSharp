using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintActivePrinterChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public OctoPrintPrinter? NewPrinter;
        public OctoPrintPrinter? OldPrinter;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
