using System.Collections.ObjectModel;

namespace AndreasReitberger.Models
{
    public class OctoPrintModelsChangedEventArgs : OctoPrintEventArgs
    {
        public ObservableCollection<OctoPrintModel> NewModels { get; set; } = new ObservableCollection<OctoPrintModel>();
    }
}
