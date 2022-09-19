using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintEventSessionChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public object Sesson { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
