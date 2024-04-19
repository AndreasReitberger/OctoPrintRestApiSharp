using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    [Obsolete("Use RestEventArgs instead")]
    internal class OctoPrintRestEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Uri? Uri { get; set; }
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
