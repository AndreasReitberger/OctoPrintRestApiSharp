using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public class OctoPrintEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; } = string.Empty;
        public int CallbackId { get; set; } = -1;
        public string SessonId { get; set; } = string.Empty;
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
