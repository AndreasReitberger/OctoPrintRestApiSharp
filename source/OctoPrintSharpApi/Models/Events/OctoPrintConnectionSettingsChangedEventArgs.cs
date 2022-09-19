using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintConnectionSettingsChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public OctoPrintConnectionSettings NewConnectionSettings { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
