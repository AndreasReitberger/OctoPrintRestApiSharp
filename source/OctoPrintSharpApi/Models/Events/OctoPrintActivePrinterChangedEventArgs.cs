using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintActivePrinterChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public OctoPrintPrinter NewPrinter { get; set; }
        public OctoPrintPrinter OldPrinter { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
