using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintActivePrintInfoChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public OctoPrintJobInfo? NewActivePrintInfo;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
