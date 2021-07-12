using AndreasReitberger.Models.Settings;

namespace AndreasReitberger.Models
{
    public class OctoPrintPrinterConfigChangedEventArgs : OctoPrintEventArgs
    {
        public OctoPrintSettings NewConfiguration { get; set; }
    }
}
