using Newtonsoft.Json;
using System;
using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.OctoPrint.Models
{
    [Obsolete("Use JsonConvertEventArgs instead")]
    public class OctoPrintJsonConvertEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public string OriginalString { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
