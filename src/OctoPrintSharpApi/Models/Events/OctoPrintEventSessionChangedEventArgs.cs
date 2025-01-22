using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    [Obsolete("Use SessionChangedEventArgs instead")]
    public class OctoPrintEventSessionChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public object? Session;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
