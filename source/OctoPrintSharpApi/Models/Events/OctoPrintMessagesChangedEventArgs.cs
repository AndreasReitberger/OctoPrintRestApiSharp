using Newtonsoft.Json;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintMessagesChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public object RepetierMessage { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
