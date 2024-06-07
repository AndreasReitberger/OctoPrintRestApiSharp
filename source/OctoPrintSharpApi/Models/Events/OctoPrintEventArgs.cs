using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.OctoPrint.Models
{
    [Obsolete("Use Print3dBaseEventArgs instead")]
    internal class OctoPrintEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; } = string.Empty;
        public int CallbackId { get; set; } = -1;
        public string SessionId { get; set; } = string.Empty;
        public string Printer { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
