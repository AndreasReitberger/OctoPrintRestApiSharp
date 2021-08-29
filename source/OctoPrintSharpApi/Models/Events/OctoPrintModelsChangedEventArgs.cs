using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Models
{
    public class OctoPrintModelsChangedEventArgs : OctoPrintEventArgs
    {
        #region Properties
        public ObservableCollection<OctoPrintModel> NewModels { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
