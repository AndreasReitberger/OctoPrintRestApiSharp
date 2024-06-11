using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    [Obsolete("Use JobFinishedEventArgs instead")]
    public class OctoPrintJobFinishedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public object? Job;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
