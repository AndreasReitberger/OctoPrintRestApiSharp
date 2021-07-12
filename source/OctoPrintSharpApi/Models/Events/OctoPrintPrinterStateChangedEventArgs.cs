
namespace AndreasReitberger.Models
{
    public class OctoPrintPrinterStateChangedEventArgs : OctoPrintEventArgs
    {
        public OctoPrintPrinterState NewPrinterState { get; set; }
    }
}
