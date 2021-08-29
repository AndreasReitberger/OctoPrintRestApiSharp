using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Models
{
    public class OctoPrintActivePrintInfosChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public ObservableCollection<object> NewActivePrintInfos { get; set; } = new ObservableCollection<object>();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
