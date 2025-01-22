using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    [Obsolete("Use ActivePrinterChangedEventArgs instead")]
    public class OctoPrintActivePrinterChangedEventArgs : Print3dBaseEventArgs
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
