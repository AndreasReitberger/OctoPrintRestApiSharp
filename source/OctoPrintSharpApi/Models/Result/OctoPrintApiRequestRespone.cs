namespace AndreasReitberger.Models
{
    public class OctoPrintApiRequestRespone
    {
        public string Result { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = false;
        public bool Succeeded { get; set; } = false;
        public bool HasAuthenticationError { get; set; } = false;

        public OctoPrintRestEventArgs EventArgs { get; set; }
    }
}
