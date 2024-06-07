using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.OctoPrint.Models
{
    public class OctoPrintModelsChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public ObservableCollection<OctoPrintModel> NewModels { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
