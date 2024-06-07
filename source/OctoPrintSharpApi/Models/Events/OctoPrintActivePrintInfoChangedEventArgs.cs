using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    [Obsolete("Use JobStatusChangedEventArgs instead")]
    public class OctoPrintActivePrintInfoChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public OctoPrintJobInfo? NewActivePrintInfo;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
