using System;

namespace AndreasReitberger.Models
{
    public class OctoPrintJsonConvertEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; }
        public string OriginalString { get; set; }
        public Exception Exception { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return string.Format("{0} ({1})", Message, OriginalString);
        }
        #endregion
    }
}
