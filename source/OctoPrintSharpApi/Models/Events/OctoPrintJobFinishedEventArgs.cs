using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class OctoPrintJobFinishedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public object Job { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
