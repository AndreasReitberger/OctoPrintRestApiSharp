using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintModelGroupsChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public ObservableCollection<string> NewModelGroups { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
