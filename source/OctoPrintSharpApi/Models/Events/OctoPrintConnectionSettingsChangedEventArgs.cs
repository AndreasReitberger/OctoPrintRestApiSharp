namespace AndreasReitberger.Models
{
    public class OctoPrintConnectionSettingsChangedEventArgs : OctoPrintEventArgs
    {
        public OctoPrintConnectionSettings NewConnectionSettings { get; set; }
    }
}
