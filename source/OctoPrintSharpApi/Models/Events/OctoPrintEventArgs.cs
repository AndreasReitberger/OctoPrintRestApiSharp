using System;

namespace AndreasReitberger.Models
{
    public class OctoPrintEventArgs : EventArgs
    {
        public string Message { get; set; } = string.Empty;
        public int CallbackId { get; set; } = -1;
        public string SessonId { get; set; } = string.Empty;
        public string Printer { get; set; } = string.Empty;
    }
}
