namespace AndreasReitberger.Models
{
    public class OctoPrintActivePrintInfoChangedEventArgs : OctoPrintEventArgs
    {
        public OctoPrintJobInfo NewActivePrintInfo { get; set; }
    }
}
