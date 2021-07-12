namespace AndreasReitberger.Models
{
    public class OctoPrintTempDataEventArgs : OctoPrintEventArgs
    {
        public object TemperatureData { get; set; }
    }
}
