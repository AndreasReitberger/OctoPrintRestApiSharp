using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class OctoPrintJobsChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public object Data { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
