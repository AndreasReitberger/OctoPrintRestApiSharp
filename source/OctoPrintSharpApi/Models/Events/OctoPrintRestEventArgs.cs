using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public class OctoPrintRestEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; }
        public string Status { get; set; }
        public Uri Uri { get; set; }
        public Exception Exception { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
