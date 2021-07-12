using System;

namespace AndreasReitberger.Models
{
    public class OctoPrintEventListeningChangedEventArgs : OctoPrintEventSessionChangedEventArgs
    {
        public bool IsListening { get; set; } = false;
        public bool IsListeningToWebSocket { get; set; } = false;
    }
}
