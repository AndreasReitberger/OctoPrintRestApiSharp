namespace AndreasReitberger.Models
{
    public class OctoPrintJobFinishedEventArgs : OctoPrintEventArgs
    {
        public object Job { get; set; }
    }
}
